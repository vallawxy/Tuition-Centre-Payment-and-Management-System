using AceTuitionPaymentSystem.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.Controllers
{
    public class ReportController : Controller
    {
        //private AceTuitionEntities objAceTuitionEntities;

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReportModal()
        {
            return PartialView();
        }

        public void DownloadExcel(string type, string year, string month)
        {

            using(var context = new AceTuitionEntities()) {

                
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var Ep = new ExcelPackage())
                {



                    ExcelWorksheet SummarySheet = Ep.Workbook.Worksheets.Add("Summary");
                    var title = "Ace Tuition Report";
                    var description = "";

                    if (type == "month")
                    {
                        string[] monthName = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                        description = "Monthly Report on " + monthName[Int16.Parse(month) - 1] + " " + year;
                    } else if (type == "quarter")
                    {
                        if(month == "q1")
                        {
                            description = "Quarter Report on First Quarter ( JAN - MAR ) " + year; 
                        } else if (month == "q2")
                        {
                            description = "Quarter Report on Second Quarter ( APR - JUN ) " + year;
                        } else if (month == "q3")
                        {
                            description = "Quarter Report on Third Quarter ( JUL - SEP ) " + year;
                        } else if (month == "q4")
                        {
                            description = "Quarter Report on Fourth Quarter ( OCT - DEC ) " + year;
                        }

                    } else if (type == "year")
                    {
                        description = "Annual Report on " + year; 
                    }

                    
                    SummarySheet.Cells["A1"].Value = title;
                    SummarySheet.Cells["A1"].Style.Font.Size = 14;
                    SummarySheet.Cells["A1"].Style.Font.Bold = true;
                    SummarySheet.Cells["A2"].Value = description;
                    SummarySheet.Cells["A3"].Value = "Generated on " + DateTime.Now.ToString();

                    SummarySheet.Cells["A5"].Value = "Payment Status";
                    SummarySheet.Cells["B5"].Value = "Quantity";
                    SummarySheet.Cells["C5"].Value = "Quantity Percentage (%)";
                    SummarySheet.Cells["D5"].Value = "Total Amount (RM)";




                    ExcelWorksheet OutstandingSheet = Ep.Workbook.Worksheets.Add("Outstanding");

                    OutstandingSheet.Cells["A1"].Value = "No.";
                    OutstandingSheet.Cells["B1"].Value = "Student Code";
                    OutstandingSheet.Cells["C1"].Value = "Student Name";
                    OutstandingSheet.Cells["D1"].Value = "Student IC";
                    OutstandingSheet.Cells["E1"].Value = "Parent Name";
                    OutstandingSheet.Cells["F1"].Value = "Contact Number";
                    OutstandingSheet.Cells["G1"].Value = "Outstanding Amount (RM)";
                    OutstandingSheet.Cells["H1"].Value = "Month";
                    OutstandingSheet.Cells["I1"].Value = "Year";

                    decimal outstandingAmount = 0;
                    int row = 2;



                    var outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding").ToList();

                    if (type == "month")
                    {
                        int yearInt = Int16.Parse(year);
                        int monthInt = Int16.Parse(month);
                        outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_month == monthInt && x.payment_year == yearInt).ToList();
                    }

                    else if (type == "quarter")
                    {
                        int yearInt = Int16.Parse(year);
                        if (month == "q1")
                        {
                            outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_month >= 1 && x.payment_month <= 3 && x.payment_year == yearInt).ToList();
                        }
                        else if (month == "q2")
                        {
                            outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_month >= 4 && x.payment_month <= 6 && x.payment_year == yearInt).ToList();
                        }
                        else if (month == "q3")
                        {
                            outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_month >= 7 && x.payment_month <= 9 && x.payment_year == yearInt).ToList();
                        }
                        else if (month == "q4")
                        {
                            outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_month >= 10 && x.payment_month <= 12 && x.payment_year == yearInt).ToList();
                        }

                    }
                    else if (type == "year")
                    {
                        int yearInt = Int16.Parse(year);
                        outstanding = context.tb_payment.Where(x => x.payment_status == "outstanding" && x.payment_year == yearInt).ToList();
                    }


                    
                    foreach (var item in outstanding)
                    {
                        var child = context.tb_child.Where(x => x.child_ic == item.child_ic).SingleOrDefault();
                        var parent = context.tb_parent.Where(x => x.parent_ic == item.parent_ic).SingleOrDefault();

                        OutstandingSheet.Cells[string.Format("A{0}", row)].Value = row - 1;
                        OutstandingSheet.Cells[string.Format("B{0}", row)].Value = child.child_code;
                        OutstandingSheet.Cells[string.Format("C{0}", row)].Value = child.child_name_eng + " " + child.child_name_chinese;
                        OutstandingSheet.Cells[string.Format("D{0}", row)].Value = child.child_ic;
                        OutstandingSheet.Cells[string.Format("E{0}", row)].Value = parent.parent_name;
                        OutstandingSheet.Cells[string.Format("F{0}", row)].Value = parent.parent_phone;
                        OutstandingSheet.Cells[string.Format("G{0}", row)].Value = item.payment_outstanding;
                        OutstandingSheet.Cells[string.Format("H{0}", row)].Value = item.payment_month;
                        OutstandingSheet.Cells[string.Format("I{0}", row)].Value = item.payment_year;

                        outstandingAmount = outstandingAmount + item.payment_outstanding;
                        row++;
                    }
                    OutstandingSheet.Cells["A:AZ"].AutoFitColumns();




                    ExcelWorksheet PaidSheet = Ep.Workbook.Worksheets.Add("Paid");

                    PaidSheet.Cells["A1"].Value = "No.";
                    PaidSheet.Cells["B1"].Value = "Receipt Number";
                    PaidSheet.Cells["C1"].Value = "Student Code";
                    PaidSheet.Cells["D1"].Value = "Student Name";
                    PaidSheet.Cells["E1"].Value = "Student IC";
                    PaidSheet.Cells["F1"].Value = "Parent Name";
                    PaidSheet.Cells["G1"].Value = "Contact Number";
                    PaidSheet.Cells["H1"].Value = "Outstanding Amount (RM)";
                    PaidSheet.Cells["I1"].Value = "Received Amount (RM)";
                    PaidSheet.Cells["J1"].Value = "Month";
                    PaidSheet.Cells["K1"].Value = "Year";

                    decimal paidAmount = 0;
                    row = 2;



                    var receipt = context.tb_receipt.Where(x => x.receipt_status == "accepted").OrderBy(x => x.receipt_code).ToList();
                    decimal receiptCount = 0;

                    foreach (var rec in receipt)
                    {
                        var item = context.tb_payment.Where(x => x.payment_id == rec.payment_id).SingleOrDefault();

                        if (type == "month")
                        {
                            int yearInt = Int16.Parse(year);
                            int monthInt = Int16.Parse(month);
                            
                            item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_month == monthInt && x.payment_year == yearInt).SingleOrDefault();
                        }
                        else if (type == "quarter")
                        {
                            int yearInt = Int16.Parse(year);
                            if (month == "q1")
                            {
                                item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_month >= 1 && x.payment_month <= 3 && x.payment_year == yearInt).SingleOrDefault();
                            }
                            else if (month == "q2")
                            {
                                item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_month >= 4 && x.payment_month <= 6 && x.payment_year == yearInt).SingleOrDefault();
                            }
                            else if (month == "q3")
                            {
                                item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_month >= 7 && x.payment_month <= 9 && x.payment_year == yearInt).SingleOrDefault();
                            }
                            else if (month == "q4")
                            {
                                item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_month >= 10 && x.payment_month <= 12 && x.payment_year == yearInt).SingleOrDefault();
                            }

                        }
                        else if (type == "year")
                        {
                            int yearInt = Int16.Parse(year);
                            item = context.tb_payment.Where(x => x.payment_id == rec.payment_id && x.payment_year == yearInt).SingleOrDefault();
                        }


                        if (item != null)
                        {
                            var child = context.tb_child.Where(x => x.child_ic == item.child_ic).SingleOrDefault();
                            var parent = context.tb_parent.Where(x => x.parent_ic == item.parent_ic).SingleOrDefault();

                            PaidSheet.Cells[string.Format("A{0}", row)].Value = row - 1;
                            PaidSheet.Cells[string.Format("B{0}", row)].Value = rec.receipt_code;

                            PaidSheet.Cells[string.Format("C{0}", row)].Value = child.child_code;
                            PaidSheet.Cells[string.Format("D{0}", row)].Value = child.child_name_eng + " " + child.child_name_chinese;
                            PaidSheet.Cells[string.Format("E{0}", row)].Value = child.child_ic;
                            PaidSheet.Cells[string.Format("F{0}", row)].Value = parent.parent_name;
                            PaidSheet.Cells[string.Format("G{0}", row)].Value = parent.parent_phone;
                            PaidSheet.Cells[string.Format("H{0}", row)].Value = item.payment_outstanding;
                            PaidSheet.Cells[string.Format("I{0}", row)].Value = item.payment_amount;
                            PaidSheet.Cells[string.Format("J{0}", row)].Value = item.payment_month;
                            PaidSheet.Cells[string.Format("K{0}", row)].Value = item.payment_year;

                            paidAmount = paidAmount + rec.receipt_outstanding;
                            row++;
                            receiptCount++;
                        }



                    }
                    PaidSheet.Cells["A:AZ"].AutoFitColumns();






                    var pending = context.tb_payment.Where(x => x.payment_status == "pending").ToList();
                    if (type == "month")
                    {
                        int yearInt = Int16.Parse(year);
                        int monthInt = Int16.Parse(month);

                        pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_month == monthInt && x.payment_year == yearInt).ToList();
                    }
                    else if (type == "quarter")
                    {
                        int yearInt = Int16.Parse(year);
                        if (month == "q1")
                        {
                            pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_month >= 1 && x.payment_month <= 3 && x.payment_year == yearInt).ToList();
                        }
                        else if (month == "q2")
                        {
                            pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_month >= 4 && x.payment_month <= 6 && x.payment_year == yearInt).ToList();

                        }
                        else if (month == "q3")
                        {
                            pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_month >= 7 && x.payment_month <= 9 && x.payment_year == yearInt).ToList();

                        }
                        else if (month == "q4")
                        {
                            pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_month >= 10 && x.payment_month <= 12 && x.payment_year == yearInt).ToList();
                        }

                    }
                    else if (type == "year")
                    {
                        int yearInt = Int16.Parse(year);
                        pending = context.tb_payment.Where(x => x.payment_status == "pending" && x.payment_year == yearInt).ToList();
                    }








                    decimal pendingAmount = 0;
                    foreach (var item in pending)
                    {
                        pendingAmount = pendingAmount + item.payment_outstanding;
                    }

                    decimal outCount = outstanding.Count;
                    decimal penCount = pending.Count;
                    decimal recCount = receiptCount;


                    var outPercent = "";
                    var penPercent = "";
                    var recPercent = "";

                    if ((outCount + penCount + recCount) != 0)
                    {
                        outPercent = (outCount / (outCount + penCount + recCount) * 100).ToString("#.##");
                        penPercent = (penCount / (outCount + penCount + recCount) * 100).ToString("#.##");
                        recPercent = (recCount / (outCount + penCount + recCount) * 100).ToString("#.##");
                    } else
                    {
                        outPercent = "0";
                        penPercent = "0";
                        recPercent = "0";
                    }
                    
                    SummarySheet.Cells["A6"].Value = "Outstanding";
                    SummarySheet.Cells["B6"].Value = outstanding.Count;
                    SummarySheet.Cells["C6"].Value = outPercent;
                    SummarySheet.Cells["D6"].Value = outstandingAmount;


                    
                    SummarySheet.Cells["A7"].Value = "Pending";
                    SummarySheet.Cells["B7"].Value = pending.Count;
                    SummarySheet.Cells["C7"].Value = penPercent;
                    SummarySheet.Cells["D7"].Value = pendingAmount;


                    
                    SummarySheet.Cells["A8"].Value = "Paid";
                    SummarySheet.Cells["B8"].Value = receiptCount;
                    SummarySheet.Cells["C8"].Value = recPercent;
                    SummarySheet.Cells["D8"].Value = paidAmount;

                    SummarySheet.Cells["A9"].Value = "Total";
                    SummarySheet.Cells["A9"].Style.Font.Bold = true;
                    SummarySheet.Cells["B9"].Value = outCount + penCount + receiptCount;
                    SummarySheet.Cells["B9"].Style.Font.Bold = true;
                    SummarySheet.Cells["C9"].Value = 100;
                    SummarySheet.Cells["C9"].Style.Font.Bold = true;
                    SummarySheet.Cells["D9"].Value = outstandingAmount + pendingAmount + paidAmount;
                    SummarySheet.Cells["D9"].Style.Font.Bold = true;


                    SummarySheet.Cells["A:AZ"].AutoFitColumns();



                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=AceTuitionReport.xlsx");

                    ////or if you use asp.net, get the relative path
                    //var InputFileName = Guid.NewGuid() + ".xlsx";
                    //string filePath = "~/UploadReport/" + InputFileName;

                    ////Write the file to the disk
                    //FileInfo fi = new FileInfo(filePath);
                    //Ep.SaveAs(fi);

                    


                    Response.BinaryWrite(Ep.GetAsByteArray());
                    Response.End();


                }


            }

        }


    }
}