# Issues with This Approach & Alternatives

## 🔹 Issue 1: Flow Runs Only on Invoice Creation
- **Problem**: If TaxRate is updated later, TotalAmount is not recalculated.
- **Solution**: Add **"When a row is modified"** trigger.

## 🔹 Issue 2: Handling Null Values
- **Problem**: If `InvoiceAmount` or `TaxRate` is empty, flow may fail.
- **Solution**: Add default values in **Power Automate expressions**.

## 🔹 Alternative Approach: Use Business Rules
- **Instead of Power Automate, we can use Business Rules in Dataverse**
  - ✅ Runs on form-level, instant execution
  - ❌ Limited flexibility compared to Power Automate
