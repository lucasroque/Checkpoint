using Checkpoint.Tools;
using System;
using System.ComponentModel;
using Checkpoint.Model;
using System.Windows.Data;
using Checkpoint.Control;

namespace Checkpoint.ViewControl
{
    class CompanyViewControl : INotifyPropertyChanged
    {
        CompanyControl companyControl = new CompanyControl();

        private string _TBName;
        private string _TBCNPJ;
        private string _TBInscription;
        private string _TBResponsible;
        private string _TBPhone;
        private string _TBZipCode;
        private string _TBFilter;
        private ICollectionView _CompanyList;

        public CompanyViewControl()
        {
            fillGridCompany();
        }

        public string TBName
        {
            get { return _TBName; }
            set
            {
                this.MutateVerbose(ref _TBName, value, RaisePropertyChanged());
            }
        }

        public string TBCNPJ
        {
            get { return _TBCNPJ; }
            set
            {
                string lastCnpj = _TBCNPJ == null ? "" : _TBCNPJ;

                this.MutateVerbose(ref _TBCNPJ, value, RaisePropertyChanged());
                _TBCNPJ = Formatter.getInstance.formatCnpj(value, lastCnpj);
            }
        }

        public string TBInscription
        {
            get { return _TBInscription; }
            set
            {
                this.MutateVerbose(ref _TBInscription, value, RaisePropertyChanged());
            }
        }

        public string TBResponsible
        {
            get { return _TBResponsible; }
            set
            {
                this.MutateVerbose(ref _TBResponsible, value, RaisePropertyChanged());
            }
        }

        public string TBPhone
        {
            get { return _TBPhone; }
            set
            {
               _TBPhone = Formatter.getInstance.formatPhoneNumber(value, _TBPhone == null ? "" : _TBPhone);
            }
        }

        public string TBZipCode
        {
            get { return _TBZipCode; }
            set
            {
                _TBZipCode = Formatter.getInstance.formatZipCode(value, _TBZipCode == null ? "" : _TBZipCode);
            }
        }

        public ICollectionView CompanyList
        {
            get { return _CompanyList; }
            set {
                this.MutateVerbose(ref _CompanyList, value, RaisePropertyChanged());
            }
        }

        public string TBFilter
        {
            get { return _TBFilter; }
            set
            {
                this.MutateVerbose(ref _TBFilter, value, RaisePropertyChanged());
                FilterCollection();
            }
        }

        public void fillGridCompany()
        {
            CompanyList = CollectionViewSource.GetDefaultView(companyControl.getAllCompanies());
            CompanyList.Filter = new Predicate<object>(Filter);
        }

        private void FilterCollection()
        {
            if (_CompanyList != null)
            {
                _CompanyList.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            Company data = (Company)obj;

            if (data is Company)
            {
                if (!string.IsNullOrEmpty(_TBFilter))
                {
                    return Util.contains(data.companyName, _TBFilter);
                }
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
