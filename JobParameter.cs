using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envanter3
{
  public  class JobParameter
    {
        private int _jobId;
        private ParameterTypes _parameterType;
        private string _parameterValue;
        private int _orderNo;

        public int JobId
        {
            get
            {
                return _jobId;
            }

            set
            {
                _jobId = value;
            }
        }

        public ParameterTypes ParameterType
        {
            get
            {
                return _parameterType;
            }

            set
            {
                _parameterType = value;
            }
        }

        public int OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
            }
        }

        public string ParameterValue
        {
            get
            {
                return _parameterValue;
            }

            set
            {
                _parameterValue = value;
            }
        }
    }
}
