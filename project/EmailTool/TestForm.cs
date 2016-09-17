using OpenPop.Mime;
using OpenPop.Mime.Header;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailTool
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = "D://";
            string plainBody = "";
            string emailDate = "";
            Pop3Client connection = new Pop3Client();
            try
            {
                connection.Connect("pop.163.com", 110, false);
                //connection.Authenticate("sxycxwb@163.com", "38668181wafl1314");
                connection.Authenticate("sxllfl@163.com", "38668181wsxwb");
                int Count = connection.GetMessageCount();
                var infoList = connection.GetMessageInfos();
                for (int intMessageNumber = 1; intMessageNumber <= Count; intMessageNumber++)
                {
                    OpenPop.Mime.Message emailMessage = connection.GetMessage(intMessageNumber); // get the full message
                    MessageHeader emailHeader = emailMessage.Headers;
                    RfcMailAddress emailFrom = emailHeader.From;
                    string fromAddress = emailFrom.Address;
                    string fromName = emailFrom.DisplayName;
                    string fromComplete = emailHeader.From.ToString();
                    emailDate = emailHeader.DateSent.ToString();
                    string conDes = emailHeader.Subject;

                    List<RfcMailAddress> emailTo = emailHeader.To;
                    // Loop through the list and concatenate the e-mail addresses

                    string strAllEmails = "";
                    foreach (RfcMailAddress tempTo in emailTo)
                    {
                        if (tempTo.HasValidMailAddress)
                        {
                            strAllEmails += tempTo.MailAddress + ",";
                        }
                    }
                    // Then get the type of body text you desire.
                    try
                    {
                        plainBody = emailMessage.FindFirstPlainTextVersion().GetBodyAsText();
                    }
                    catch { }

                    string htmlBody = emailMessage.FindFirstHtmlVersion().GetBodyAsText();
                    MessagePart emailMessagePart = connection.GetMessage(intMessageNumber).MessagePart;

                    if (emailMessagePart.IsAttachment == true)
                    {
                        foreach (MessagePart emailAttachment in emailMessage.FindAllAttachments())
                        {
                            //-- Set variables
                            string OriginalFileName = emailAttachment.FileName;
                            string ContentID = emailAttachment.ContentId; // If this is set then the attachment is inline.
                            string ContentType = emailAttachment.ContentType.MediaType; // type of attachment pdf, jpg, png, etc.

                            //-- write the attachment to the disk

                            System.IO.File.WriteAllBytes(path + OriginalFileName, emailAttachment.Body); //overwrites MessagePart.Body with attachment   

                        }
                    }
                    //System.Web.HttpContext.Current.Response.Write(fromAddress + ",," + fromName + ",," + fromComplete + ",," + strAllEmails + ",," + emailDate + ",," + conDes + ",," + htmlBody);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            connection.Disconnect();
        }
    }
}
