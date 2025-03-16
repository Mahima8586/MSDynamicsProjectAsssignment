// JavaScript source code
/**
 * Mock API to receive Order details from Dynamics 365.
 */
const express = require("express");
const app = express();

app.use(express.json());

app.post("/receiveOrder", (req, res) => {
    console.log("Received Order:", req.body);
    res.status(200).send({ message: "Order received successfully!" });
});

app.listen(3000, () => console.log("Mock API running on port 3000."));
