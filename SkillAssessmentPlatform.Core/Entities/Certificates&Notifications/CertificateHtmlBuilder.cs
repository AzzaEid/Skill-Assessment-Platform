public static class CertificateHtmlBuilder
{
    public static string Build(string fullName, string trackName, string levelName, DateTime issueDate, string verificationCode)
    {
        return $@"
        <html>
        <head>
            <title>Certificate of Completion</title>
            <style>
                body {{
                    font-family: 'Segoe UI', sans-serif;
                    background-color: #f4f4f4;
                    padding: 30px;
                }}
                .certificate {{
                    width: 800px;
                    margin: auto;
                    padding: 40px;
                    border: 10px solid #ccc;
                    background-color: #fff;
                    text-align: center;
                }}
                h1 {{ font-size: 32px; margin-bottom: 20px; }}
                .name {{ font-size: 24px; font-weight: bold; margin: 20px 0; }}
                .info {{ font-size: 18px; }}
                .code {{ margin-top: 30px; color: #777; font-size: 14px; }}
                .download-btn {{
                    display: inline-block;
                    margin-top: 40px;
                    padding: 12px 24px;
                    background-color: #007bff;
                    color: white;
                    text-decoration: none;
                    border-radius: 5px;
                    font-size: 16px;
                }}
            </style>
        </head>
        <body>
            <div class='certificate'>
                <h1>Certificate of Completion</h1>
                <p>This certifies that</p>
                <div class='name'>{fullName}</div>
                <p>has successfully completed</p>
                <div class='info'>
                    Track: <strong>{trackName}</strong><br/>
                    Level: <strong>{levelName}</strong><br/>
                    Issued on: {issueDate.ToShortDateString()}
                </div>
                <div class='code'>
                    Verification Code: {verificationCode}<br/>
                    View at: https://yourdomain.com/api/certificates/html/{verificationCode}
                </div>

                <a class='download-btn' href='https://yourdomain.com/api/certificates/{verificationCode}/pdf' target='_blank'>
                    Download PDF
                </a>
            </div>
        </body>
        </html>";
    }
}
