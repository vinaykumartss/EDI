// See https://aka.ms/new-console-template for more information
using System;
using EdiEngine.Standards.X12_004010.Maps;
using System.Linq;
using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;

Console.WriteLine("Hello, World!");

//string contents = File.ReadAllText(@"C:\Users\Vinay\Downloads\EdiEngine-develop\EdiEngine-develop\EdiEngine.Tests\TestData\940.OK.edi");

//EdiDataReader r = new EdiDataReader();
//EdiBatch b1 = r.FromString(contents);
////Write Json using newtonsoft
////check no exception
//JsonDataWriter dd = new JsonDataWriter();
//string xx = dd.WriteToString(b1);
//JsonConvert.SerializeObject(b1);
//JsonConvert.SerializeObject(b1.Interchanges[0].Groups[0].Transactions[0]);

////or use writer to write to string or stream
//JsonDataWriter w = new JsonDataWriter();
//string str = w.WriteToString(b1);
//Console.WriteLine(str);



string edi = @"ISA*00*          *00*          *ZZ*VENDOR         *ZZ*WAREHOUSE      *090902*0900*U*00401*000009221*0*P*>
GS*OW*VENDOR*WAREHOUSE*20090902*1700*9221*X*004010
ST*940*20066
W05*N*35131*POMAIL
SE*3*20066
GE*1*9221
IEA*1*000009221";

//Your custom map assembly name in ctor
EdiDataReader r = new EdiDataReader("EDIConsole");
EdiBatch b = r.FromString(edi);
EdiTrans t = b.Interchanges[0].Groups[0].Transactions[0];
Console.WriteLine(new JsonDataWriter().WriteToString(b));
Console.Read();