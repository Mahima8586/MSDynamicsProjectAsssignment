/**
 * @file opportunityRevenueCalculation.test.js
 * @description Unit test cases for updateEstimatedRevenue function in Opportunity Form.
 * Uses Jest framework to validate expected behavior of JavaScript function.

 */

// ✅ Import the function to test
const { updateEstimatedRevenue } = require('../src/scripts/opportunityRevenueCalculation.js');

// ✅ Mock form context and its getAttribute method
const mockFormContext = {
    getAttribute: jest.fn(),
};

// ✅ Reset mock before each test to prevent conflicts
beforeEach(() => {
    mockFormContext.getAttribute.mockReset();
});

describe('updateEstimatedRevenue Function Tests', () => {

    test('✅ Should disable Estimated Revenue for Fixed Price', () => {
        // 🔹 Simulate Opportunity Type selection as "Fixed Price"
        mockFormContext.getAttribute.mockImplementation((field) => {
            switch (field) {
                case 'new_opportunitytype':
                    return { getValue: () => 'Fixed Price' };
                case 'new_estimatedrevenue':
                    return { setDisabled: jest.fn(), setValue: jest.fn() };
                default:
                    return null;
            }
        });

        // 🔹 Call function
        updateEstimatedRevenue(mockFormContext);

        // 🔹 Assertions: Check if Estimated Revenue is disabled
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(true);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(null);
    });

    test('✅ Should enable and calculate Estimated Revenue for Variable Price', () => {
        // 🔹 Simulate Opportunity Type selection as "Variable Price" with sample values
        mockFormContext.getAttribute.mockImplementation((field) => {
            switch (field) {
                case 'new_opportunitytype':
                    return { getValue: () => 'Variable Price' };
                case 'new_totalunits':
                    return { getValue: () => 10 };
                case 'new_unitprice':
                    return { getValue: () => 100 };
                case 'new_discount':
                    return { getValue: () => 50 };
                case 'new_estimatedrevenue':
                    return { setDisabled: jest.fn(), setValue: jest.fn() };
                default:
                    return null;
            }
        });

        // 🔹 Call function
        updateEstimatedRevenue(mockFormContext);

        // 🔹 Expected Calculation: (10 * 100) - 50 = 950
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(false);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(950);
    });

    test('❌ Should handle missing Estimated Revenue field gracefully', () => {
        // 🔹 Simulate scenario where Estimated Revenue field is missing
        mockFormContext.getAttribute.mockImplementation((field) => {
            return field === 'new_opportunitytype' ? { getValue: () => 'Variable Price' } : null;
        });

        // 🔹 Expect function NOT to throw errors
        expect(() => updateEstimatedRevenue(mockFormContext)).not.toThrow();
    });

    test('✅ Should keep Estimated Revenue editable if Opportunity Type is undefined', () => {
        // 🔹 Simulate missing Opportunity Type
        mockFormContext.getAttribute.mockImplementation((field) => {
            switch (field) {
                case 'new_opportunitytype':
                    return { getValue: () => null };
                case 'new_estimatedrevenue':
                    return { setDisabled: jest.fn(), setValue: jest.fn() };
                default:
                    return null;
            }
        });

        // 🔹 Call function
        updateEstimatedRevenue(mockFormContext);

        // 🔹 Assertions
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(false);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(null);
    });

});
