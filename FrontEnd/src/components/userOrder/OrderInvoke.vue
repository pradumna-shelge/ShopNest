<template>
    <div>
        <button @click="generatePDF">Generate PDF</button>
    </div>
</template>

<script setup>
import { PDFDocument } from 'pdf-lib';

const address = {
    country: "India",
    state: "Maharashtra",
    city: "Pune",
    firstName: "Pradumna",
    lastName: "shelage",
    addressLine: "2B New House",
    zip: "413529",
};

const productData = [
    {
        price: "450.78",
        mrprice: "560.56",
        quantity: 1,
        productName: "Gucci GG Blooms Supreme slide sandals",
        description: "A floral slide sandal in our GG Blooms supreme canvas with a molded rubber footbed. GG Blooms Supreme canvas; Molded rubber footbed; Flat; Made in Italy",
    }
];

const generatePDF = async () => {
    const pdfDoc = await PDFDocument.create();
    const page = pdfDoc.addPage([600, 400]);

    page.drawText('Your Company Name', {
        x: 160,
        y: 360,
        size: 12,
    });

    page.drawText('Shipping Address:', {
        x: 50,
        y: 270,
        size: 18,
    });
    page.drawText(`Name: ${address.firstName} ${address.lastName}`, {
        x: 50,
        y: 245,
        size: 12,
    });
    page.drawText(`Address: ${address.addressLine}`, {
        x: 50,
        y: 225,
        size: 12,
    });
    page.drawText(`City: ${address.city}, State: ${address.state}, Zip: ${address.zip}, Country: ${address.country}`, {
        x: 50,
        y: 205,
        size: 12,
    });

    page.drawText('Product Details:', {
        x: 50,
        y: 170,
        size: 18,
    });

    const cellWidth = 275;
    const cellHeight = 20;

    page.drawText("Product Name", {
        x: 50,
        y: 155,
        size: 12,
    });
    page.drawText("Description", {
        x: 50 + cellWidth + 5,
        y: 155,
        size: 12,
    });

    productData.forEach((product, index) => {
        const rowY = 135 - index * cellHeight;

        page.drawText(product.productName, {
            x: 50,
            y: rowY,
            size: 12,
        });

        page.drawText(product.description, {
            x: 50 + cellWidth + 5,
            y: rowY,
            size: 12,
            maxWidth: cellWidth,
        });
    });

    const pdfBytes = await pdfDoc.save();
    const pdfBlob = new Blob([pdfBytes], { type: 'application/pdf' });

    const downloadLink = document.createElement('a');
    downloadLink.href = URL.createObjectURL(pdfBlob);
    downloadLink.download = 'Document.pdf';
    downloadLink.click();
};
</script>
