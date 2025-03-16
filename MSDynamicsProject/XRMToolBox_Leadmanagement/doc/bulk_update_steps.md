# XRM Toolbox: Bulk Updating Leads to "Qualified" Status

## ✅ Step 1: Open XRM Toolbox & Connect to Dynamics 365
1. Open **XRM Toolbox** → Click **Connect**.
2. Select your **Dynamics 365 environment** → Click **Connect**.

## ✅ Step 2: Open Bulk Data Updater Plugin
1. In the **XRM Toolbox Plugin Store**, search for **"Bulk Data Updater."**
2. Click **Install** → Open the plugin.

## ✅ Step 3: Load Leads Entity Data
1. Select **Table Name: Leads (lead)**
2. Apply a **filter**:  
   - **Status (statecode) = Open (0)**  
   - **Max Records: 50** (to limit updates to 50 leads).
3. Click **Retrieve Data**.

## ✅ Step 4: Configure Bulk Update
1. Select **Field to Update:**  
   - **Field Name:** `statecode`  
   - **New Value:** `Qualified (1)`  
2. Click **Preview Changes**.

## ✅ Step 5: Execute Bulk Update
1. Click **Update Records** → Confirm changes.
2. Wait for execution to complete.

## ✅ Step 6: Verify the Update
1. Open **Power Apps / Dataverse**.
2. Check the **Lead Status field** in the updated records.
3. Take **before and after screenshots**.

