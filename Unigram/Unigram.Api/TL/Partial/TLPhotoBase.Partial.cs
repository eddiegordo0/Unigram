﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Api.Helpers;

namespace Telegram.Api.TL
{
    public partial class TLPhotoBase : INotifyPropertyChanged
    {
        public virtual TLInputPhotoBase ToInputPhoto()
        {
            throw new NotImplementedException();
        }

        #region Download/upload

        private double _uploadingProgress;
        public double UploadingProgress
        {
            get
            {
                return _uploadingProgress;
            }
            set
            {
                _uploadingProgress = value;
                RaisePropertyChanged(() => UploadingProgress);
                RaisePropertyChanged(() => Progress);
            }
        }

        private double _downloadingProgress;
        public double DownloadingProgress
        {
            get
            {
                return _downloadingProgress;
            }
            set
            {
                _downloadingProgress = value;
                RaisePropertyChanged(() => DownloadingProgress);
                RaisePropertyChanged(() => Progress);
            }
        }

        public double Progress
        {
            get
            {
                if (_downloadingProgress > 0)
                {
                    return _downloadingProgress;
                }

                return _uploadingProgress;
            }
        }

        public double LastProgress { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public override void RaisePropertyChanged(string propertyName)
        {
            Execute.OnUIThread(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        public Progress<double> Download()
        {
            DownloadingProgress = 0.02;

            return new Progress<double>((value) =>
            {
                DownloadingProgress = value;
                Debug.WriteLine(value);
            });
        }

        public Progress<double> Upload()
        {
            UploadingProgress = 0.02;

            return new Progress<double>((value) =>
            {
                UploadingProgress = value;
                Debug.WriteLine(value);
            });
        }

        #endregion
    }

    public partial class TLPhoto
    {
        public override TLInputPhotoBase ToInputPhoto()
        {
            return new TLInputPhoto { Id = Id, AccessHash = AccessHash };
        }
    }

    public partial class TLPhotoEmpty
    {
        public override TLInputPhotoBase ToInputPhoto()
        {
            return new TLInputPhotoEmpty();
        }
    }
}
