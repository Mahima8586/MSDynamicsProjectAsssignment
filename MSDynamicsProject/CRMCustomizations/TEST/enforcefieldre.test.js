
const { enforceFieldRequirements } = require('./validation.js'); 


const mockFormContext = {
    getAttribute: jest.fn(),
};

// Reset mock before each test
beforeEach(() => {
    mockFormContext.getAttribute.mockReset();
});

describe('enforceFieldRequirements Function Tests', () => {
    test('? Should make Email required when Preferred Contact is "Email"', () => {
        // Mocking Preferred Contact selection
        mockFormContext.getAttribute.mockImplementation((fieldName) => {
            switch (fieldName) {
                case 'new_preferredmethodofcontact':
                    return { getValue: () => 'Email' };
                case 'new_email':
                    return { setRequiredLevel: jest.fn() };
                case 'new_phone':
                    return { setRequiredLevel: jest.fn() };
                default:
                    return null;
            }
        });

        enforceFieldRequirements(mockFormContext);

        // Verify email is set as required
        expect(mockFormContext.getAttribute('new_email').setRequiredLevel).toHaveBeenCalledWith('required');
        expect(mockFormContext.getAttribute('new_phone').setRequiredLevel).toHaveBeenCalledWith('none');
    });

    test('? Should make Phone required when Preferred Contact is "Phone"', () => {
        mockFormContext.getAttribute.mockImplementation((fieldName) => {
            switch (fieldName) {
                case 'new_preferredmethodofcontact':
                    return { getValue: () => 'Phone' };
                case 'new_email':
                    return { setRequiredLevel: jest.fn() };
                case 'new_phone':
                    return { setRequiredLevel: jest.fn() };
                default:
                    return null;
            }
        });

        enforceFieldRequirements(mockFormContext);

        expect(mockFormContext.getAttribute('new_email').setRequiredLevel).toHaveBeenCalledWith('none');
        expect(mockFormContext.getAttribute('new_phone').setRequiredLevel).toHaveBeenCalledWith('required');
    });

    test('? Should not require any fields if Preferred Contact is not set', () => {
        mockFormContext.getAttribute.mockImplementation((fieldName) => {
            switch (fieldName) {
                case 'new_preferredmethodofcontact':
                    return { getValue: () => null };
                case 'new_email':
                    return { setRequiredLevel: jest.fn() };
                case 'new_phone':
                    return { setRequiredLevel: jest.fn() };
                default:
                    return null;
            }
        });

        enforceFieldRequirements(mockFormContext);

        expect(mockFormContext.getAttribute('new_email').setRequiredLevel).toHaveBeenCalledWith('none');
        expect(mockFormContext.getAttribute('new_phone').setRequiredLevel).toHaveBeenCalledWith('none');
    });

    test('? Should handle missing fields without errors', () => {
        mockFormContext.getAttribute.mockImplementation((fieldName) => {
            if (fieldName === 'new_preferredmethodofcontact') {
                return { getValue: () => 'Email' };
            }
            return null;
        });

        expect(() => enforceFieldRequirements(mockFormContext)).not.toThrow();
    });
});
