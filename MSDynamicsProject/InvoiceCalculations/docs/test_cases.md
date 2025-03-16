# Test Cases for Invoice Total Calculation Flow

## ✅ Test 1: Invoice with Tax
- **Input**: InvoiceAmount = `$1000`, TaxRate = `10% (0.10)`
- **Expected Output**: TotalAmount = `$1100`
- **Validation**: Verify `TotalAmount` field in Dataverse.

## ✅ Test 2: Invoice with 0% Tax
- **Input**: InvoiceAmount = `$500`, TaxRate = `0%`
- **Expected Output**: TotalAmount = `$500`
- **Validation**: Invoice should remain unchanged.

## ✅ Test 3: High Tax Rate
- **Input**: InvoiceAmount = `$200`, TaxRate = `25%`
- **Expected Output**: TotalAmount = `$250`
