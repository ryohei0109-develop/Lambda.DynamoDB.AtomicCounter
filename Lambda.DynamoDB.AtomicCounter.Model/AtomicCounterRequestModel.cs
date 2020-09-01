using Amazon;

namespace Lambda.DynamoDB.AtomicCounter.Model
{
    public class AtomicCounterRequestModel
    {
        /// <summary>
        /// テーブル名を取得/設定します。
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// テーブルのハッシュ属性名を取得/設定します。
        /// </summary>
        public string HashName
        {
            get;
            set;
        }

        /// <summary>
        /// テーブルのハッシュ属性値を取得/設定します。
        /// </summary>
        public string HashValue
        {
            get;
            set;
        }

        /// <summary>
        /// 指定しない場合は「"Version"」を使用します。
        /// </summary>
        public string CounterFieldName
        {
            get
            {
                if (string.IsNullOrEmpty(_fieldName) == true)
                {
                    return DefaultFieldName;
                }
                else
                {
                    return _fieldName;
                }
            }
            set
            {
                _fieldName = value;
            }
        }

        public string AccessKey
        {
            get;
            set;
        }

        public string SecretKey
        {
            get;
            set;
        }

        private string _fieldName;

        private const string DefaultFieldName = "Version";
    }
}
