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
  <a href="#overview">
    <b>Overview</b>
  </a>
  ·
  <a href="#installation">
    <b>Installation</b>
  </a>
  ·
  <a href="#usage">
    <b>Usage</b>
  </a>
  ·
  <a href="#support">
    <b>Support</b>
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

## Getting Started

The 1Password CLI PAM Provider is used by Command to resolve PAM-eligible credentials for Universal Orchestrator extensions and for accessing Certificate Authorities. When configured, Command will use the 1Password CLI PAM Provider to retrieve credentials needed to communicate with the target system. There are two ways to install the 1Password CLI PAM Provider, and you may elect to use one or both methods:

1. **Locally on the Keyfactor Command server**: PAM credential resolution via the 1Password CLI PAM Provider will occur on the Keyfactor Command server each time an eligible credential is needed.
2. **Remotely on Universal Orchestrators**: When Jobs are dispatched to Universal Orchestrators, the associated Certificate Store extension assembly will use the 1Password CLI PAM Provider to resolve eligible PAM credentials.

Before proceeding with installation, you should consider which pattern is best for your requirements and use case.

### Installation

> [!IMPORTANT]
> For the most up-to-date and complete documentation on how to install a PAM provider extension, please visit our [product documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/ReferenceGuide/Preparing%20Third%20Party%20PAM%20Providers%20to%20Work%20with.htm?Highlight=pam%20provider#InstallingCustomPAMProviderExtensions).

To install 1Password CLI PAM Provider, it is recommended you install [kfutil](https://github.com/Keyfactor/kfutil). `kfutil` is a command-line tool that simplifies the process of creating PAM Types in Keyfactor Command.

#### Overview

The 1Password CLI PAM Provider uses the 1Password CLI to communicate with 1Password in PowerShell. It does not support using the 1Password SDKs or 1Password Connect Server APIs.
It does not require additional licensing for any services in 1Password besides basic level features.
Communication with 1Password uses Service Account and associated Token. Service Account Tokens are tied to specific Vaults when they are created, and will need to be regenerated if additional Vault access needs to be added later.

This PAM Provider supports retrieving all fields available in 1Password, such as usernames and passwords. It can be installed on either the Keyfactor Command Platform or on Universal Orchestrators.

#### Requirements

   In order to use this PAM Provider extension, the 1Password CLI must be installed.

   Refer to the [1Password CLI documentation](https://developer.1password.com/docs/cli/get-started/) for how to install the CLI and add it to the execution path.
   After the CLI is installed, it is prudent to verify that the integration will be able to reach it, by opening a new PowerShell terminal and typing the simple command `op`. This should not result in an error and instead show the top-level help info for the CLI.

   A Service Account also needs to be created and configured with a Service Account Token. Refer to the [1Password Service Accounts documentation](https://developer.1password.com/docs/service-accounts/get-started/) for how to set up and provision a Service Account.
   Please note that Service Account Tokens are associated with Vaults at time of creation. If additional Vaults are later added that need to be accessed, the Token will need to be reconfigured to be granted acccess to additional Vaults.

   Since this extension expects to be able to run the CLI in a PowerShell session, the account running the Keyfactor service that uses this PAM Provider will need to be able to access and use PowerShell.

#### Create PAM type in Keyfactor Command

##### Using `kfutil`

Create the required PAM Types in the connected Command platform.

```shell
# 1Password-CLI
kfutil pam-types create -r 1password-cli-pam -n 1Password-CLI
```

##### Using the API

For full API docs please visit our [product documentation](https://software.keyfactor.com/Core-OnPrem/Current/Content/WebAPI/KeyfactorAPI/PAMProvidersPOSTTypes.htm?Highlight=pam%20type).

Below is the payload to `POST` to the Keyfactor Command API:

```json
{
  "Name": "1Password-CLI",
  "Parameters": [
    {
      "Name": "Vault",
      "DisplayName": "1Password Secret Vault",
      "DataType": 1,
      "InstanceLevel": false,
      "Description": "The name of the Vault in 1Password."
    },
    {
      "Name": "Token",
      "DisplayName": "1Password Service Account Token",
      "DataType": 2,
      "InstanceLevel": false,
      "Description": "The Service Account Token that is configured to access the specified Vault."
    },
    {
      "Name": "Item",
      "DisplayName": "1Password Item Name",
      "DataType": 1,
      "InstanceLevel": true,
      "Description": "The name of the credential item in 1Password. This could be the name of a Login object or a Password object."
    },
    {
      "Name": "Field",
      "DisplayName": "Field Name on Item",
      "DataType": 1,
      "InstanceLevel": true,
      "Description": "The name of the Field to retrieve from the specified Item. For a Login, this would be 'username' or 'password'. For an API Credential this would be 'credential'."
    }
  ]
}
```

#### Install PAM provider on Keyfactor Command Host (Local)

1. On the server that hosts Keyfactor Command, download and unzip the latest release of the 1Password CLI PAM Provider from the [Releases](../../releases) page.

2. Copy the assemblies to the appropriate directories on the Keyfactor Command server:

    <details><summary>Keyfactor Command 11+</summary>

    1. Copy the unzipped assemblies to each of the following directories:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\Extensions\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\Extensions\1password-cli-pam`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\Extensions\1password-cli-pam`

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
            <register type="IPAMProvider" mapTo="Keyfactor.Extensions.Pam._1Password.CliPam, 1password-cli-pam" name="1Password-CLI" />
        </container>
        ```

    4. Repeat steps 2 and 3 for each of the directories listed in step 1. The configuration files are located in the following paths by default:

        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebAgentServices\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\KeyfactorAPI\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\WebConsole\web.config`
        * `C:\Program Files\Keyfactor\Keyfactor Platform\Service\CMSTimerService.exe.config`

    </details>

3. Restart the Keyfactor Command services (`iisreset`).

#### Install PAM provider on a Universal Orchestrator Host (Remote)

1. Install the 1Password CLI PAM Provider assemblies.

    * **Using kfutil**: On the server that hosts the Universal Orchestrator, run the following command:

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
    {
      "Keyfactor:PAMProviders:1Password-CLI:InitializationInfo": {
        "Vault": "<Vault>",
        "Token": "<Token>"
      }
    }
    ```

    Populate the fields in this object with credentials and configuration data collected in the [requirements](docs/1password-cli.md#requirements) section.

3. Restart the Universal Orchestrator service.

### Usage

#### From Keyfactor Command Host (Local)

##### Define a PAM provider in Command

1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Privileged Access Management**.

2. Select the **Add** button to create a new PAM provider. Click the dropdown for **Provider Type** and select **1Password-CLI**.

   > [!IMPORTANT]
   > If you're running Keyfactor Command 11+, make sure `Remote Provider` is unchecked.

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

#### From a Universal Orchestrator Host (Remote)

<details><summary>Keyfactor Command 11+</summary>

##### Define a remote PAM provider in Command

In Command 11 and greater, before using the 1Password-CLI PAM type, you must define a Remote PAM Provider in the Command portal.

1. In the Keyfactor Command Portal, hover over the ⚙️  (settings) icon in the top right corner of the screen and select **Privileged Access Management**.

2. Select the **Add** button to create a new PAM provider.

3. Make sure that `Remote Provider` is checked.

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
{
  "Item": "<Item>",
  "Field": "<Field>"
}
```

Replace each `<placeholder>` with the value described for that instance parameter in the table above.

> We recommend creating this JSON object in a text editor, and copying it into the Secret Value field.

</details>

> [!NOTE]
> Additional information on 1Password-CLI can be found in the [supplemental documentation](docs/1password-cli.md).

#### Extension Mechanics

The 1Password CLI PAM Provider uses the 1Password CLI to retrieve credential information. It executes `op read` commands in PowerShell sessions that return that credential information.
The Service Account Token, which acts as an authentication string, is entered into each PowerShell session that is started.

## Support

The 1Password CLI PAM Provider is supported by Keyfactor for Keyfactor customers. If you have a support issue, please open a support ticket via the Keyfactor Support Portal at https://support.keyfactor.com.

> To report a problem or suggest a new feature, use the **[Issues](../../issues)** tab. If you want to contribute actual bug fixes or proposed enhancements, use the **[Pull requests](../../pulls)** tab.

## License

Apache License 2.0, see [LICENSE](LICENSE)

## Related Integrations

See all [Keyfactor PAM Provider extensions](https://github.com/orgs/Keyfactor/repositories?q=pam).
