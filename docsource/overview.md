## Overview

The 1Password CLI PAM Provider uses the 1Password CLI to communicate with 1Password in PowerShell. It does not support using the 1Password SDKs or 1Password Connect Server APIs.
It does not require additional licensing for any services in 1Password besides basic level features.
Communication with 1Password uses Service Account and associated Token. Service Account Tokens are tied to specific Vaults when they are created, and will need to be regenerated if additional Vault access needs to be added later.

This PAM Provider supports retrieving all fields available in 1Password, such as usernames and passwords. It can be installed on either the Keyfactor Command Platform or on Universal Orchestrators.