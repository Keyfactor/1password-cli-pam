// Copyright 2024 Keyfactor
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Keyfactor.Logging;
using Keyfactor.Platform.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Keyfactor.Extensions.Pam._1Password
{
    public class CliPam : IPAMProvider
    {
        public string Name => "1Password-CLI";

        public string GetPassword(Dictionary<string, string> instanceParameters, Dictionary<string, string> initializationInfo)
        {
            ILogger logger = LogHandler.GetClassLogger<CliPam>();

            logger.LogTrace("Reading \"Token\" parameter");
            var token = initializationInfo["Token"];
            logger.LogDebug("Read \"Token\" parameter");

            logger.LogTrace("Reading \"Vault\" parameter");
            var vault = initializationInfo["Vault"];
            logger.LogDebug($"Read \"Vault\" parameter: {vault}");

            logger.LogTrace("Reading \"Item\" parameter");
            var item = instanceParameters["Item"];
            logger.LogDebug($"Read \"Item\" parameter: {item}");

            logger.LogTrace("Reading \"Field\" parameter");
            var field = instanceParameters["Field"];
            logger.LogDebug($"Read \"Field\" parameter: {field}");

            var cliTokenCommand = "$Env:OP_SERVICE_ACCOUNT_TOKEN = \"{0}\"";
            var execEnv = string.Format(cliTokenCommand, token);
            logger.LogDebug("Prepared PowerShell command to set Service Account Token");

            var cliReadCommand = "op read op://\"{0}\"/\"{1}\"/\"{2}\"";
            var execRead = string.Format(cliReadCommand, vault, item, field);
            logger.LogDebug($"Prepared 1Password PowerShell command for retrieving field: {execRead}");

            string result;
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                logger.LogTrace("Opening PowerShell runspace");
                runspace.Open();
                try
                {
                    using (PowerShell ps = PowerShell.Create())
                    {
                        ps.Runspace = runspace;
                        ps.AddScript(execEnv);
                        ps.AddScript(execRead);

                        logger.LogTrace("Invoking PowerShell commands");
                        var results = ps.Invoke();

                        if (ps.HadErrors)
                        {
                            logger.LogInformation("The PowerShell command generated an error while running the 1Password Command. Reading error...");
                            if (ps.Streams.Error.Count == 0)
                            {
                                string message = "The 1Password PowerShell command indicated an error occurred, but did not return any errors to read.";
                                logger.LogError(message);
                                throw new Exception(message);
                            }

                            var powershellException = ps.Streams.Error[0].Exception;
                            logger.LogError(powershellException, "Running the 1Password PowerShell command generated an error.");
                            throw powershellException;
                        }

                        else if (results.Count == 0)
                        {
                            string message = "The 1Password PowerShell command did not return any errors, but did not return any results. Unable to continue.";
                            logger.LogError(message);
                            throw new Exception(message);
                        }

                        logger.LogTrace("Reading PowerShell result");
                        result = results[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while running the 1Password PowerShell command.");
                    throw;
                }
                finally
                {
                    logger.LogTrace("Closing PowerShell runspace");
                    runspace.Close();
                }
            }

            return result;
        }
    }
}
