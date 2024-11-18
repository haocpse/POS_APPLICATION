using ScannerQRCode.Entities;
using ScannerQRCode.Repositories;
using System.Drawing;
using System.Windows;
using ZXing;
using ZXing.Windows.Compatibility;

namespace ScannerQRCode.Services
{

    public class QRCodeService
    {
        private QRCodeRepository QRcodeRepository = new QRCodeRepository();

        public void AddQRCode(QRCodeScan qRCodeScan)
        {
            QRcodeRepository.Add(qRCodeScan);
        }

        public Result ProcessQRCode(Bitmap bitmap)
        {
            BarcodeReader barcodeReader = new BarcodeReader
            {
                Options = new ZXing.Common.DecodingOptions
                {
                    PossibleFormats = new List<ZXing.BarcodeFormat>
                    {
                        ZXing.BarcodeFormat.QR_CODE,           // Quét mã QR
                        ZXing.BarcodeFormat.CODE_128,          // Quét mã vạch 128
                        ZXing.BarcodeFormat.CODE_39,           // Quét mã vạch 39
                        ZXing.BarcodeFormat.EAN_13,            // Quét mã EAN 13
                        ZXing.BarcodeFormat.UPC_A,             // Quét mã UPC
                        ZXing.BarcodeFormat.EAN_8,             // Quét mã EAN 8
                        ZXing.BarcodeFormat.ITF,               // Quét mã vạch ITF (Interleaved 2 of 5)
                        ZXing.BarcodeFormat.CODABAR,           // Quét mã Codabar
                        ZXing.BarcodeFormat.DATA_MATRIX,       // Quét mã Data Matrix
                        ZXing.BarcodeFormat.PDF_417,           // Quét mã PDF 417
                        ZXing.BarcodeFormat.AZTEC               // Quét mã Aztec
                      
                    },
                  
                }
            };
            Result result = barcodeReader.Decode(bitmap);
            return result;
        }
        public bool IsURL(string qrContent)
        {
            if (Uri.IsWellFormedUriString(qrContent, UriKind.Absolute))
            {
                return true;
            }
            return false;
        }



    }
}
