using System;
using System.Collections.Generic;
using System.Text;
using TwinCAT.Ads;

    internal class NotificationRequestEntry
    {
        private AmsAddress   _rAddr;        // the AmsNetId of the requestor
        private uint         _indexGroup;   // the requested index group
        private uint         _indexOffset;  // the requested index offset
        private uint         _cbLength;     // the number of bytes to send
        private AdsTransMode _transMode;    // the transmission mode for this notification
        private uint         _maxDelay;     // maximum delay until notification should be sent
        private uint         _cycleTime;    // cycle time for checking changes

        internal NotificationRequestEntry(AmsAddress rAddr,
                                          uint indexGroup,
                                          uint indexOffset,
                                          uint cbLength,
                                          AdsTransMode transMode,
                                          uint maxDelay,
                                          uint cycleTime)
        {
            _rAddr = rAddr;
            _indexGroup = indexGroup;
            _indexOffset = indexOffset;
            _cbLength = cbLength;
            _transMode = transMode;
            _maxDelay = maxDelay;
            _cycleTime = cycleTime;
        }

    }

