using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using NaverPayAPI.Controllers;
using NaverPayAPI.Models;

namespace NaverPayAPI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] //vvvvvvv
        static void Main()
        {
            //네이버 측에서 TLS 버전 업 요청 (기존 tls10, 11로 호출하고 있었음)
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            RealThing();
            NonRealThing();
        }

        static void RealThing()
        {
            for(int i = 0; i < 10; i++)
            {
                // 실물상품
                SaDataController controller = new SaDataController();
                //RequestHeader header = new RequestHeader("sKiGL3fZMN8_X9khwhsW", "EqHLg9ZV5w", "MGV0anFPbzYxL2p"); // test
                RequestHeader header = new RequestHeader("z3KY8ouAzQxjWfQG2cne", "9HqMLwAVBX", "T0dYNXYwUzhmcXh"); // 운영
                SaReviewData data = new SaReviewData()
                {
                    startTime = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") + "000000"
                    //startTime = "20240723000000"
                    ,
                    endTime = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") + "235959"
                    //endTime = "20240723235959"
                    ,
                    rowsPerPage = 100
                    ,
                    pageNumber = 1 + i
                };
                //controller.PostData(header, "https://dev.apis.naver.com/np_kjtjj853051/naverpay/payments/v2.2/list/history", data, ""); // test
                controller.PostData(header, "https://apis.naver.com/np_kjtjj853051/naverpay/payments/v2.2/list/history", data, "Y");  // 운영

            }
        }

        static void NonRealThing()
        {
            for (int i = 0; i < 10; i++)
            {
                // 비실물상품
                SaDataController controller = new SaDataController();
                //RequestHeader header = new RequestHeader("sKiGL3fZMN8_X9khwhsW", "EqHLg9ZV5w", "MGV0anFPbzYxL2p"); // test
                RequestHeader header = new RequestHeader("z3KY8ouAzQxjWfQG2cne", "9HqMLwAVBX", "TDMrblhPbUp2UXF"); // 운영
                SaReviewData data = new SaReviewData()
                {
                    startTime = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") + "000000"
                    //startTime = "20240101000000"
                    ,
                    endTime = DateTime.Today.AddDays(-1).ToString("yyyyMMdd") + "235959"
                    //endTime = "20240101235959"
                    ,
                    rowsPerPage = 100
                    ,
                    pageNumber = 1 + i
                };
                //controller.PostData(header, "https://dev.apis.naver.com/np_kjtjj853051/naverpay/payments/v2.2/list/history", data, ""); // test
                controller.PostData(header, "https://apis.naver.com/np_kjtjj853051/naverpay/payments/v2.2/list/history", data, "N");  // 운영
            }
        }
    }
}