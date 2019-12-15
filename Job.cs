using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envanter3
{
    public class Job
    {
        private long _id;
        private string _modulSinifAdi;
        private string _methodAdi;
        private DateTime _calistirmaZamani;
        private List<JobParameter> _parameters;

        public long Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string ModulSinifAdi
        {
            get
            {
                return _modulSinifAdi;
            }

            set
            {
                _modulSinifAdi = value;
            }
        }

        public string MethodAdi
        {
            get
            {
                return _methodAdi;
            }

            set
            {
                _methodAdi = value;
            }
        }

        public DateTime CalistirmaZamani
        {
            get
            {
                return _calistirmaZamani;
            }

            set
            {
                _calistirmaZamani = value;
            }
        }

        public List<JobParameter> Parameters
        {
            get
            {
                return _parameters;
            }

            set
            {
                _parameters = value;
            }
        }

        public object[] GetObjectParameters()
        {
            if (this._parameters != null && this._parameters.Count > 0)
            {
                List<object> objectList = new List<object>();
                //this._parameters

                foreach (var item in this._parameters.OrderBy(a => a.OrderNo))
                {
                    object obj = null;
                    try
                    {
                        switch (item.ParameterType)
                        {
                            case ParameterTypes.String:
                                obj = item.ParameterValue.ToString();
                                break;
                            case ParameterTypes.Number:
                                obj = Convert.ToInt32(item.ParameterValue);
                                break;
                            case ParameterTypes.Boolean:
                                obj = Convert.ToBoolean(item.ParameterValue);
                                break;
                            case ParameterTypes.DateTime:
                                obj = Convert.ToDateTime(item.ParameterValue);
                                break;
                        }
                    }
                    catch (Exception ){ }
                    finally { objectList.Add(obj); }
                }

                return objectList.ToArray();
            }
            else

                return null;
        }
    }
}
