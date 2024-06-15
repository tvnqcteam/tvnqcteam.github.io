using System;
using System.Data.OleDb;

namespace BarcodeScannerApp
{
    public partial class Default : System.Web.UI.Page
    {
        private OleDbConnection connection;
        private string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\10.0.0.236\public\6. Cross section\Cross Section Database\Backup\Barcode 4MC OPEN FILE.mdb;";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize connection
            connection = new OleDbConnection(connectionString);
        }

        protected void btnScan_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();

            if (!string.IsNullOrEmpty(barcode))
            {
                try
                {
                    // Check and close connection if open
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    // Open connection
                    connection.Open();

                    // 1. Check if barcode exists in dbo_QC_QE_Tolist
                    string query1 = "SELECT [CircuitNumberBarcode], [4MCNo] FROM dbo_QC_QE_Tolist WHERE [CircuitNumberBarcode] = @Barcode";
                    OleDbCommand command1 = new OleDbCommand(query1, connection);
                    command1.Parameters.AddWithValue("@Barcode", barcode);
                    OleDbDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read())
                    {
                        string mc4Number = reader1["4MCNo"].ToString();
                        reader1.Close();

                        // 2. Check if mc4Number exists in dbo_QC_Document_Registration
                        string query2 = "SELECT [DocNo], [FileLink] FROM dbo_QC_Document_Registration WHERE [DocNo] = @DocNo";
                        OleDbCommand command2 = new OleDbCommand(query2, connection);
                        command2.Parameters.AddWithValue("@DocNo", mc4Number);
                        OleDbDataReader reader2 = command2.ExecuteReader();

                        if (reader2.Read())
                        {
                            string fileLink = reader2["FileLink"].ToString();
                            reader2.Close();

                            // Display the PDF in the iframe
                            pdfViewer.Attributes["src"] = fileLink;
                            lblMessage.Text = "PDF loaded successfully.";

                            // Reset the barcode TextBox
                            txtBarcode.Text = string.Empty;
                        }
                        else
                        {
                            lblMessage.Text = "No matching DocNo found in dbo_QC_Document_Registration.";
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Barcode not found in dbo_QC_QE_Tolist.";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
                finally
                {
                    // Ensure connection is closed after operation
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                lblMessage.Text = "Please enter a barcode.";
            }
        }
    }
}
