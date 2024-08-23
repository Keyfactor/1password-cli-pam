<h1 align="center" style="border-bottom: none">
    1Password CLI PAM Provider
</h1>

<p align="center">
  <!-- Badges -->
<img src="https://img.shields.io/badge/integration_status-production-3D1973?style=flat-square" alt="Integration Status: production" />
<a href="https://github.com/Keyfactor/1password-cli-pam/releases"><img src="https://img.shields.io/github/v/release/Keyfactor/1password-cli-pam?style=flat-square" alt="Release" /></a>
<img src="https://img.shields.io/github/issues/Keyfactor/1password-cli-pam?style=flat-square" alt="Issues" />
<img src="https://img.shields.io/github/downloads/Keyfactor/1password-cli-pam/total?style=flat-square&label=downloads&color=28B905" alt="GitHub Downloads (all assets, all releases)" />
</p>

<p align="center">
  <!-- TOC -->
  <a href="#support">
    <b>Support</b>
  </a> 
  ·
  <a href="#getting-started">
    <b>Installation</b>
  </a>
  ·
  <a href="#license">
    <b>License</b>
  </a>
  ·
  <a href="https://github.com/orgs/Keyfactor/repositories?q=pam">
    <b>Related Integrations</b>
  </a>
</p>

## Overview

The 1Password CLI PAM Provider uses the 1Password CLI to communicate with 1Password in PowerShell. It does not support using the 1Password SDKs or 1Password Connect Server APIs.
It does not require additional licensing for any services in 1Password besides basic level features.
Communication with 1Password uses Service Account and associated Token. Service Account Tokens are tied to specific Vaults when they are created, and will need to be regenerated if additional Vault access needs to be added later.

This PAM Provider supports retrieving all fields available in 1Password, such as usernames and passwords. It can be installed on either the Keyfactor Command Platform or on Universal Orchestrators.

## Support
The 1Password CLI PAM Provider is supported by Keyfactor for Keyfactor customers. If you have a support issue, please open a support ticket with your Keyfactor representative. If you have a support issue, please open a support ticket via the Keyfactor Support Portal at https://support.keyfactor.com. 

> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## Getting Started

The 1Password CLI PAM Provider is used by Command to resolve PAM-eligible credentials for Universal Orchestrator extensions and for accessing Certificate Authorities. When configured, Command will use the 1Password CLI PAM Provider to retrieve credentials needed to communicate with the target system. There are two ways to install the 1Password CLI PAM Provider, and you may elect to use one or both methods:

1. **Locally on the Keyfactor Command server**: PAM credential resolution via the 1Password CLI PAM Provider will occur on the Keyfactor Command server each time an elegible credential is needed.
2. **Remotely On Universal Orchestrators**: When Jobs are dispatched to Universal Orchestrators, the associated Certificate Store extension assembly will use the 1Password CLI PAM Provider to resolve eligible PAM credentials.

Before proceeding with installation, you should consider which pattern is best for your requirements and use case.

### Installation

To install 1Password CLI PAM Provider, you must install [kfutil](https://github.com/Keyfactor/kfutil). Kfutil is a command-line tool that simplifies the process of creating PAM Types in Keyfactor Command, among many other useful automation features.







#### Prerequisites

1. Follow the [requirements section](docs/1password-cli.md#requirements) to configure a Service Account, grant necessary API permissions, and create secrets.

    <details><summary>Requirements</summary>
    In order to use this PAM Provider extension, the 1Password CLI must be installed.

    Refer to the [1Password CLI documentation](https://developer.1password.com/docs/cli/get-started/) for how to install the CLI and add it to the execution path.
    After the CLI is installed, it is prudent to verify that the integration will be able to reach it, by opening a new PowerShell terminal and typing the simple command `op`. This should not result in an error and instead show the top-level help info for the CLI.

    A Service Account also needs to be created and configured with a Service Account Token. Refer to the [1Password Service Accounts documentation](https://developer.1password.com/docs/service-accounts/get-started/) for how to set up and provision a Service Account.
    Please note that Service Account Tokens are associated with Vaults at time of creation. If additional Vaults are later added that need to be accessed, the Token will need to be reconfigured to be granted acccess to additional Vaults.

    Since this extension expects to be able to run the CLI in a PowerShell session, the account running the Keyfactor service that uses this PAM Provider will need to be able to access and use PowerShell.

    </details>

2. Use kfutil to create the required PAM Types in the connected Command platform.

    ```shell
    # 1Password-CLI
    kfutil pam types-create -r 1password-cli-pam -n 1Password-CLI
    ```

#### Install on Keyfactor Command (Local)



1. On the server that hosts Keyfactor Command, download and unzip the latest release of the 1Password CLI PAM Provider from the [Releases](../../releases) page.

2. Copy the assemblies to the appropriate directories on the Keyfactor Command server:

    <details><summary>Keyfactor Command 11+</summary>

    1. Copy the unzipped assemblies to each of the following directories:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\Extensions\PamProviders\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\Extensions\PamProviders\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\Extensions\PamProviders`

    </details>

    <details><summary>Keyfactor Command 10</summary>

    1. Copy the assemblies to each of the following directories:
    
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\bin\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\bin\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\bin\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\Service\1password-cli-pam`

    2. Open a text editor on the Keyfactor Command server as an administrator and open the `web.config` file located in the `WebAgentServices` directory.

    3. In the `web.config` file, locate the `<container> </container>` section and add the following registration:

        ```xml
        <container>
            ...
            <!--The following are PAM Provider registrations. Uncomment them to use them in the Keyfactor Product:-->
            
            <!--Add the following line exactly to register the PAM Provider-->
            <register type="IPAMProvider" mapTo="Keyfactor.Extensions.Pam._1Password.CliPam, Keyfactor.Command.PAMProviders" name="1Password-CLI" />
        </container>
        ```

    4. Repeat steps 2 and 3 for each of the directories listed in step 1. The configuration files are located in the following paths by default:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\Service\CMSTimerService.exe.config`

    </details>

3. Restart the Keyfactor Command services (`iisreset`).




#### Install on a Universal Orchestrator (Remote)


1. Install the 1Password CLI PAM Provider assemblies.

    * **Using kfutil**: On the server that that hosts the Universal Orchestrator, run the following command:

        ```shell
        # Windows Server
        kfutil orchestrator extension -e 1password-cli-pam@latest --out "C:\Program Files\Keyfactor\Keyfactor Orchestrator\extensions"

        # Linux
        kfutil orchestrator extension -e 1password-cli-pam@latest --out "/opt/keyfactor/orchestrator/extensions"
        ```

    * **Manually**: Download the latest release of the 1Password CLI PAM Provider from the [Releases](../../releases) page. Extract the contents of the archive to:

        * **Windows Server**: `C:\Program Files\Keyfactor\Keyfactor Orchestrator\extensions\1password-cli-pam`
        * **Linux**: `/opt/keyfactor/orchestrator/extensions/1password-cli-pam`

2. Included in the release is a `manifest.json` file that contains the following object:

    ```json
    // 1password-cli-pam/manifest.json

    {
        "Keyfactor:PAMProviders:1Password-CLI:InitializationInfo": {
            "Vault": "orchestrator-secrets",
            "Token": "xxxxxx"
        }
    }

    ```

    Populate the fields in this object with credentials and configuration data collected in the [requirements](docs/1password-cli.md#requirements) section.

3. Restart the Universal Orchestrator service.









### Usage






#### Keyfactor Command (Local)



##### Define a PAM provider in Command
1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Priviledged Access Management**.

2. Select the **Add** button to create a new PAM provider. Click the dropdown for **Provider Type** and select **1Password-CLI**.

    > If you're running Keyfactor Command 11+, make sure "Remote Provider" is unchecked.

3. Populate the fields with the necessary information collected in the [requirements](docs/1password-cli.md#requirements) section:

| Initialization parameter | Display Name | Description |
| --- | --- | --- |
| Vault | 1Password Secret Vault | The name of the Vault in 1Password. |
| Token | 1Password Service Account Token | The Service Account Token that is configured to access the specified Vault. |


4. Click **Save**. The PAM provider is now available for use in Keyfactor Command.

##### Using the PAM provider

Now, when defining Certificate Stores (**Locations**->**Certificate Stores**), **1Password-CLI** will be available as a PAM provider option. When defining new Certificate Stores, the secret parameter form will display tabs for **Load From Keyfactor Secrets** or **Load From PAM Provider**. 

Select the **Load From PAM Provider** tab, choose the **1Password-CLI** provider from the list of **Providers**, and populate the fields with the necessary information from the table below:

| Instance parameter | Display Name | Description |
| --- | --- | --- |
| Item | 1Password Item Name | The name of the credential item in 1Password. This could be the name of a Login object or a Password object. |
| Field | Field Name on Item | The name of the Field to retrieve from the specified Item. For a Login, this would be 'username' or 'password'. For an API Credential this would be 'credential'. |





#### Universal Orchestrator (Remote)



<details><summary>Keyfactor Command 11+</summary>

##### Define a remote PAM provider in Command

In Command 11 and greater, before using the 1Password-CLI PAM type, you must define a Remote PAM Provider in the Command portal.

1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Priviledged Access Management**.

2. Select the **Add** button to create a new PAM provider.

3. Make sure that "Remote Provider" is checked.

4. Click the dropdown for **Provider Type** and select **1Password-CLI**. 

5. Give the provider a unique name.

6. Click "Save".

##### Using the PAM provider

When defining Certificate Stores (**Locations**->**Certificate Stores**), **1Password-CLI** can be used as a PAM provider. When defining a new Certificate Store, the secret parameter form will display tabs for **Load From Keyfactor Secrets** or **Load From PAM Provider**.

Select the **Load From PAM Provider** tab, choose the **1Password-CLI** provider from the list of **Providers**, and populate the fields with the necessary information from the table below:

| Instance parameter | Display Name | Description |
| --- | --- | --- |
| Item | 1Password Item Name | The name of the credential item in 1Password. This could be the name of a Login object or a Password object. |
| Field | Field Name on Item | The name of the Field to retrieve from the specified Item. For a Login, this would be 'username' or 'password'. For an API Credential this would be 'credential'. |


</details>

<details><summary>Keyfactor Command 10</summary>

When defining Certificate Stores (**Locations**->**Certificate Stores**), **1Password-CLI** can be used as a PAM provider.

When entering Secret fields, select the **Load From Keyfactor Secrets** tab, and populate the **Secret Value** field with the following JSON object:

```json
{"Item": "The name of the credential item in 1Password. This could be the name of a Login object or a Password object.","Field": "The name of the Field to retrieve from the specified Item. For a Login, this would be 'username' or 'password'. For an API Credential this would be 'credential'."}

```

> We recommend creating this JSON object in a text editor, and copying it into the Secret Value field.

</details>






> Additional information on 1Password-CLI can be found in the [supplimental documentation](docs/1password-cli.md).



## License

Apache License 2.0, see [LICENSE](LICENSE)

## Related Integrations

See all [Keyfactor PAM Provider extensions](https://github.com/orgs/Keyfactor/repositories?q=pam).