﻿using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using DFe.DocumentosEletronicos.Wsdl;
using DFe.Http.Ext;

namespace DFe.Http
{
    public class RequestWS
    {
        public string EnviaSefaz(DFeSoapConfig soapConfig)
        {
            try
            {
                string XMLRetorno = string.Empty;
                string xmlSoap = new Envelope().Construir(soapConfig);

                Uri uri = new Uri(soapConfig.Url);

                WebRequest webRequest = WebRequest.Create(uri);
                HttpWebRequest httpWR = (HttpWebRequest)webRequest;
                httpWR.Timeout = soapConfig.TimeOut == 0 ? 2000 : soapConfig.TimeOut;

                httpWR.ContentLength = Encoding.ASCII.GetBytes(xmlSoap).Length;

                httpWR.ClientCertificates.Add(soapConfig.Certificado);

                httpWR.ComposeContentType("application/soap+xml", Encoding.UTF8, soapConfig.Metodo);

                httpWR.Method = "POST";

                Stream reqStream = httpWR.GetRequestStream();
                StreamWriter streamWriter = new StreamWriter(reqStream);
                streamWriter.Write(xmlSoap, 0, Encoding.ASCII.GetBytes(xmlSoap).Length);
                streamWriter.Close();

                WebResponse webResponse = httpWR.GetResponse();
                Stream respStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(respStream);

                XMLRetorno = streamReader.ReadToEnd();

                return XMLRetorno;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
                throw;
            }
        }
    }
}