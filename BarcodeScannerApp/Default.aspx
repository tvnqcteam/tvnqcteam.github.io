<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BarcodeScannerApp.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Barcode Scanner App</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:TextBox ID="txtBarcode" runat="server" Width="300px"></asp:TextBox>
            <asp:Button ID="btnScan" runat="server" Text="Scan" OnClick="btnScan_Click" />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <div>
                <iframe id="pdfViewer" runat="server" style="width: 100%; height: 1100px; border: 2px solid #ccc;"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
