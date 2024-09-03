using Cesco.FW.Global.DBAdapter;
using NaverPayAPI.Models;
using NaverPayAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NaverPayAPI.Controllers
{
    public class SaDataController
    {

        //public JToken PostData(RequestHeader header, string path, string Type)
        public JToken PostData(RequestHeader header, string path, object reqBody, string Type)
        {

            string chk = string.Empty;
            string realyn = Type;

            DataTable dt = new DataTable();
            dt.Columns.Add("SEQ", typeof(int));
            dt.Columns.Add("MERCHANTPAYKEY", typeof(string));
            dt.Columns.Add("ADMISSIONYMDT", typeof(string));
            dt.Columns.Add("ADMISSIONTYPECODE", typeof(string));
            dt.Columns.Add("PRIMARYPAYMEANS", typeof(string));
            dt.Columns.Add("TOTALPAYAMOUNT", typeof(int));
            dt.Columns.Add("NPOINTPAYAMOUNT", typeof(int));
            dt.Columns.Add("TOTALCOMMISSIONAMOUNT", typeof(int));
            dt.Columns.Add("USECFMYMDT", typeof(string));

            var client = new RestClient($@"{ path }");
            var request = new RestRequest(Method.POST);
            string reqBodyJson = Newtonsoft.Json.JsonConvert.SerializeObject(reqBody);

            // body 영역
            request.AddParameter("application/json", reqBodyJson, ParameterType.RequestBody);

            // 헤더 정보
            request.AddHeader("X-Naver-Client-Id", header.clientId);
            request.AddHeader("X-Naver-Client-Secret", header.clientSecret);
            request.AddHeader("X-NaverPay-Chain-Id", header.chainId);

            IRestResponse response = client.Execute(request);

            JObject result = JObject.Parse(response.Content);

            Test obj = JsonConvert.DeserializeObject<Test>(response.Content);

            //var rHeader = result.GetValue("header").ToObject<ResponseHeader>();

            for (int i = 0; i < obj.body.list.Count; i++)
            {
                DataRow row = dt.NewRow();


                row["SEQ"] = i + 1;
                row["MERCHANTPAYKEY"] = obj.body.list[i].merchantPayKey;
                row["ADMISSIONYMDT"] = obj.body.list[i].admissionYmdt;
                row["ADMISSIONTYPECODE"] = obj.body.list[i].admissionTypeCode;
                row["PRIMARYPAYMEANS"] = obj.body.list[i].primaryPayMeans;
                row["TOTALPAYAMOUNT"] = obj.body.list[i].totalPayAmount;
                row["NPOINTPAYAMOUNT"] = obj.body.list[i].npointPayAmount;
                row["TOTALCOMMISSIONAMOUNT"] = obj.body.list[i].settleInfo.totalCommissionAmount;
                row["USECFMYMDT"] = obj.body.list[i].useCfmYmdt;

                dt.Rows.Add(row);
            }

            try
            {
                DataSet oDs = new DataSet();

                DBAdapters db = new DBAdapters();
                db.LocalInfo = new Cesco.FW.Global.DBAdapter.LocalInfo("21297", System.Reflection.MethodBase.GetCurrentMethod());
                //db.BindingConfig.ServerUriString = "http://cesnetdev.cesco.biz/WCF/IISService/WcfCommon/WcfCommonNew.svc";
                db.BindingConfig.ServerUriString = "http://cesnet.cesco.biz/WCF/IISService/WcfCommon/WcfCommonNew.svc";
                db.BindingConfig.SendTimeout = new TimeSpan(0, 15, 0);
                db.BindingConfig.CloseTimeout = new TimeSpan(0, 15, 0);
                db.BindingConfig.TimeOut = ConfigurationDetail.TimeOuts.MINUTE10;
                db.Procedure.ProcedureName = "CESCO_ACCOUNT.dbo.USP_CSN_SET_TRANSACTION_NPAY";
                db.Procedure.ParamAdd("@DATATABLE", "DBO.UDT_SET_NAVERPAYAPI_PAYLIST", dt);
                db.Procedure.ParamAdd("@REALYN", Type);

                oDs = db.ProcedureToDataSetCompress();

                chk = oDs.Tables[0].Rows[0]["CHK"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 문제가 발생하였습니다.\r\n" + ex.ToString(), "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }

            // 값 확인
            if (((int)response.StatusCode).ToString().StartsWith("2"))
            {
                return result.GetValue("body");
            }
            else
            {
                throw new Exception("");
            }
        }
    }

    public class Test
    {
        public string code { get; set; }
        public string message { get; set; }

        public NaverBody body { get; set; }
    }

    public class NaverBody
    {
        public List<NaverList> list;
        public int totalCount { get; set; }
        public int responseCount { get; set; }
    }

    public class NaverList
    {
        public string paymentId { get; set; }
        public string merchantPayKey { get; set; }
        public string admissionYmdt { get; set; }
        public string primaryPayMeans { get; set; }
        public string admissionTypeCode { get; set; }
        public int totalPayAmount { get; set; }
        public int npointPayAmount { get; set; }
        public string useCfmYmdt { get; set; }

        public NaverSettleInfoList settleInfo { get; set; }
    }
    public class NaverSettleInfoList
    {
        public int totalCommissionAmount { get; set; }
    }
}