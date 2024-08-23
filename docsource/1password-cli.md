## Overview

The 1Password CLI PAM Provider uses the 1Password CLI to communicate with 1Password in PowerShell. It does not support using the 1Password SDKs or 1Password Connect Server APIs.
It does not require additional licensing for any services in 1Password besides basic level features.
Communication with 1Password uses Service Account and associated Token. Service Account Tokens are tied to specific Vaults when they are created, and will need to be regenerated if additional Vault access needs to be added later.

This PAM Provider supports retrieving all fields available in 1Password, such as usernames and passwords. It can be installed on either the Keyfactor Command Platform or on Universal Orchestrators.

## Requirements

In order to use this PAM Provider extension, the 1Password CLI must be installed.

Refer to the [1Password CLI documentation](https://developer.1password.com/docs/cli/get-started/) for how to install the CLI and add it to the execution path.
After the CLI is installed, it is prudent to verify that the integration will be able to reach it, by opening a new PowerShell terminal and typing the simple command `op`. This should not result in an error and instead show the top-level help info for the CLI.

A Service Account also needs to be created and configured with a Service Account Token. Refer to the [1Password Service Accounts documentation](https://developer.1password.com/docs/service-accounts/get-started/) for how to set up and provision a Service Account.
Please note that Service Account Tokens are associated with Vaults at time of creation. If additional Vaults are later added that need to be accessed, the Token will need to be reconfigured to be granted acccess to additional Vaults.

Since this extension expects to be able to run the CLI in a PowerShell session, the account running the Keyfactor service that uses this PAM Provider will need to be able to access and use PowerShell.

## Extension Mechanics

The 1Password CLI PAM Provider uses the 1Password CLI to retrieve credential information. It executes `op read` commands in PowerShell sessions that return that credential information.
The Service Account Token, which acts as an authentication string, is entered into each PowerShell session that is started.

