/**
 * @file opportunityRevenueCalculation.js
 * @description Ensures "Estimated Revenue" is disabled for Fixed Price
 * and auto-calculated for Variable Price in the Opportunity Form.

 * @usage Attach this script to:
 *   - OnLoad Event of the form
 *   - OnChange Events of "Opportunity Type", "Total Units", "Unit Price", "Discount"
 */

function updateEstimatedRevenue(executionContext) {
    // ✅ Ensure the function is called with execution context
    if (!executionContext) {
        console.error("Execution Context is missing. Ensure that this function is triggered by an event.");
        return;
    }

    // ✅ Get the form context to access field attributes
    var formContext = executionContext.getFormContext();

    // ✅ Retrieve the selected Opportunity Type
    // This should be a dropdown field (Choice/Option Set)
    var opportunityType = formContext.getAttribute("new_opportunitytype")?.getValue();

    // ✅ Retrieve the Estimated Revenue field (Numeric field)
    var estimatedRevenueField = formContext.getAttribute("new_estimatedrevenue");

    // ✅ Retrieve other input fields required for calculation
    var totalUnits = formContext.getAttribute("new_totalunits")?.getValue() || 0; // Default to 0 if null
    var unitPrice = formContext.getAttribute("new_unitprice")?.getValue() || 0; // Default to 0 if null
    var discount = formContext.getAttribute("new_discount")?.getValue() || 0; // Default to 0 if null

    // ✅ Ensure Estimated Revenue field exists in the form
    if (!estimatedRevenueField) {
        console.error("Estimated Revenue field is missing on the form.");
        return;
    }

    // 🔹 Scenario 1: If Opportunity Type is "Fixed Price"
    if (opportunityType === "Fixed Price") {
        estimatedRevenueField.setDisabled(true);  // Disable the field so users cannot modify it
        estimatedRevenueField.setValue(null);     // Clear any existing value
        console.log("Estimated Revenue is disabled because Opportunity Type is Fixed Price.");
    }
    // 🔹 Scenario 2: If Opportunity Type is "Variable Price"
    else if (opportunityType === "Variable Price") {
        estimatedRevenueField.setDisabled(false); // Enable the field
        var calculatedRevenue = (totalUnits * unitPrice) - discount; // Apply formula
        estimatedRevenueF
