# Configure Webhook for Order Creation in Dynamics 365

##  Step 1: Open Power Automate
- Go to **Power Automate** > Click **Create** > **Automated Cloud Flow**
- Select **"When a row is added"** (Dataverse)
- Choose **Table: Orders** > Scope: Organization

## Step 2: Call Azure Function
- Add **"HTTP POST Request"** as an action
- URL: Paste Azure Function URL
- Headers: 
  - Content-Type: application/json
