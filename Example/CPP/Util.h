#ifndef __UTIL_HEADER__
#define __UTIL_HEADER__

time_t ConvertToUTCTime( time_t time ); //local -> utc
void ConvertTimeToDate(time_t nDate, COleDateTime& dtTime);
time_t ConvertDateToTime(COleDateTime dtTime);
time_t ConvertToLocalTime( time_t utcTime ); //utc->local


#endif //#ifndef __UTIL_HEADER__