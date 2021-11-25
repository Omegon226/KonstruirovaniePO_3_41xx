using BLL.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PDFCreator
    {
        public void CreatePDF(int TicketNomber, TicketModel Ticket, CruisesForWindowInfo CruiseInfo, CruisesForWindowInfo CheckedCruiseToBuy)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            gfx.DrawString("Билет на рейс", new XFont("Arial", 40, XFontStyle.Bold), XBrushes.Black, new XPoint(150, 70));

            int currentYPosition = 180;

            gfx.DrawString("Информация о билете", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Номер билета в очереди заказа: " + TicketNomber.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("ФИО: " + Ticket.FullName, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Индентификационная информация: " + Ticket.IdentificationInformation, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Номер сиденья: " + Ticket.SeatNumberOnTheTransport.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Дата отправки рейса: " + Ticket.RaceDepartureTime.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 150, 0)), new XPoint(50, currentYPosition), new XPoint(500, currentYPosition));
            currentYPosition += 40;

            gfx.DrawString("Информация о рейсе", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Пункт начала движения: " + CruiseInfo.Cruise.StartPointLocalityName.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Пункт начала движения: " + CruiseInfo.Cruise.EndPointLocalityName.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Цена в Рублях: " + CruiseInfo.Cruise.FullPrice.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Время поездки: " + CruiseInfo.Cruise.FullTimeInCruise.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 150, 0)), new XPoint(50, currentYPosition), new XPoint(500, currentYPosition));
            currentYPosition += 40;

            gfx.DrawString("Информация о транспорте", new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Black, new XPoint(120, currentYPosition));
            currentYPosition += 50;
            gfx.DrawString("Кол-во мест в транспорте: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.NumberOfSeats.ToString(), new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Регистрационный номер: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.RegistrationNumber, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));
            currentYPosition += 20;
            gfx.DrawString("Марка Авто: " + CheckedCruiseToBuy.Cruise.TransportOfTheCruise.Model, new XFont("Arial", 20, XFontStyle.Bold), XBrushes.Black, new XPoint(50, currentYPosition));

            DateTime DateNow = DateTime.Now;
            string DateTimeString = DateNow.Day.ToString() + DateNow.Month.ToString() + DateNow.Year.ToString();
            Random rnd = new Random();

            document.Save(@"E:\\УНИВЕР\\3-41(5 семестр (3 курс))(Смирнов)\\Конструирование ПО\\Интерфейс\\Интерфейс\\Tickets\\Ticket" + TicketNomber.ToString() + "-" + DateTimeString + "-" + rnd.Next(100000, 999999).ToString() + ".pdf");
        }
    }
}
