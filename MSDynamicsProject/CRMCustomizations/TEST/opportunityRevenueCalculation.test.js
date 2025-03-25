
const { updateEstimatedRevenue } = require('../src/scripts/opportunityRevenueCalculation.js');
const mockFormContext = {
    getAttribute: jest.fn(),
};

beforeEach(() => {
    mockFormContext.getAttribute.mockReset();
});

describe('updateEstimatedRevenue Function Tests', () => {

    test('✅ Should disable Estimated Revenue for Fixed Price', () => {
     
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

       
        updateEstimatedRevenue(mockFormContext);

        
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(true);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(null);
    });

    test('✅ Should enable and calculate Estimated Revenue for Variable Price', () => {
        
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

      
        updateEstimatedRevenue(mockFormContext);

       
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(false);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(950);
    });

    test('Should handle missing Estimated Revenue field gracefully', () => {
       
        mockFormContext.getAttribute.mockImplementation((field) => {
            return field === 'new_opportunitytype' ? { getValue: () => 'Variable Price' } : null;
        });

       
        expect(() => updateEstimatedRevenue(mockFormContext)).not.toThrow();
    });

    test('Should keep Estimated Revenue editable if Opportunity Type is undefined', () => {
        
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

        
        updateEstimatedRevenue(mockFormContext);

        
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setDisabled).toHaveBeenCalledWith(false);
        expect(mockFormContext.getAttribute('new_estimatedrevenue').setValue).toHaveBeenCalledWith(null);
    });

});
