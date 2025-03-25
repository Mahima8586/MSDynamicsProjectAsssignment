function opportunityTypeOnChange(executionContext) {
    var formContext = executionContext.getFormContext();
    const opportunityType = formContext.getAttribute("new_opportunitytype").getValue();
    const estRevenueAttr = formContext.getAttribute("estimatedvalue");
    const estRevenueCtrl = formContext.getControl("estimatedvalue");

    if (opportunityType === 1) { // Fixed Price
        estRevenueAttr.setValue(null);
        estRevenueCtrl.setDisabled(true);
    } else if (opportunityType === 2) { // Variable Price
        estRevenueCtrl.setDisabled(true);
        let units = formContext.getAttribute("new_totalunits").getValue() || 0;
        let unitPrice = formContext.getAttribute("new_unitprice").getValue() || 0;
        let discount = formContext.getAttribute("new_discount").getValue() || 0;
        let est_rev = calculateEstimatedRevenue(units, unitPrice, discount);

        formContext.getAttribute("estimatedvalue").setValue(est_rev);
    } else {
        estRevenueCtrl.setDisabled(false);
    }
}

function calculateEstimatedRevenue(units, un_price, disc) {
    return (units * un_price) - disc;

}
