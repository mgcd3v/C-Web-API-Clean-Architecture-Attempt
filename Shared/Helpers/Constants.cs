namespace Shared.Helpers
{
    public static class Constants
    {
        /* General */
        public const string FE_RESPONSE_URL = "";
        public const string PAGINATION_HEADER_NAME = "X-Pagination";

        public const string NEWPASSWORDROUTE = "newpassword";
        public const string CHANGEPASSWORDROUTE = "changepassword";

        public const string EMAILVERIFICATIONCODESUBJECT = "";
        public const string EMAILTEMPLATEFOLDERNAME = "";
        public const string SENDCODETEMPLATEFILENAME = "";

        public const int RANDOMDIGITMINVALUE = 0;
        public const int RANDOMDIGITMAXVALUE = 0;

        public const string SP_PARAMS_DATEFORMAT = "MM/dd/yyyy";
        public const string SP_PARAMS_DATETIMEFORMAT = "MM/dd/yyyy HH:mm";
        public const string SP_PARAMS_TIMEFORMAT = "HH:mm";

        public const string SP_PARAM_NULLVALUE = "NULL";

        /* Cmb Fields */
        public const string CMB_LEAVESTATUS = "LEAVE_STATUS";

        /* Messages */
        public const string MSG_CMBNOTFOUND = "Cmb not found";
        public const string MSG_CMBEXISTS = "Cmb already exists";

        /* SP Process Flags */
        public const string SP_CREATEPROCESSFLAG = "C";
        public const string SP_READPROCESSFLAG = "R";
        public const string SP_UPDATEPROCESSFLAG = "U";
        public const string SP_DELETEPROCESSFLAG = "D";

        /* SP Names */
        public const string SP_CREATEGENCMB = "SP_GEN_CMB";
        public const string SP_READGENCMB = "SP_GEN_CMB";
        public const string SP_UPDATEGENCMB = "SP_GEN_CMB";
        public const string SP_DELETEGENCMB = "SP_GEN_CMB";
    }
}
