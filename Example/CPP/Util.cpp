
#include "stdafx.h"
#include "Util.h"


time_t ConvertToUTCTime( time_t time ) //local -> utc
{
    time_t utcTime;
    struct tm uTm;
    
    gmtime_s( &uTm, &time );
    utcTime = mktime( &uTm );

    if (uTm.tm_isdst)
    	utcTime -= 3600;

    if(utcTime == -1)
        utcTime = 0;

    return utcTime;
}

void ConvertTimeToDate(time_t nDate, COleDateTime& dtTime)
{
    struct tm tmDateTime = {0};
    localtime_s( &tmDateTime, &nDate );

    int strYear= tmDateTime.tm_year + 1900;
    int strMonth = tmDateTime.tm_mon + 1;
    int strDay = tmDateTime.tm_mday;
    int strHour = 0;
    if(nDate > 0)
        strHour = tmDateTime.tm_hour;
    int strMinute = tmDateTime.tm_min;
    int strSecond = tmDateTime.tm_sec;

    dtTime.SetDateTime(strYear, strMonth, strDay, strHour, strMinute, strSecond );
}

time_t ConvertDateToTime(COleDateTime dtTime)
{
    struct tm tmpDateTime = {0};
    time_t nTime = time(0);

    localtime_s(&tmpDateTime, &nTime);

    tmpDateTime.tm_year = dtTime.GetYear() - 1900;
    tmpDateTime.tm_mon  = dtTime.GetMonth() - 1; 
    tmpDateTime.tm_mday  = dtTime.GetDay();
    tmpDateTime.tm_hour  = dtTime.GetHour();
    tmpDateTime.tm_min  = dtTime.GetMinute();
    tmpDateTime.tm_sec  = dtTime.GetSecond();
    tmpDateTime.tm_isdst = -1;

    nTime = mktime(&tmpDateTime);
    if(nTime == -1)
        nTime = 0;
    return nTime;
}

time_t ConvertToLocalTime( time_t utcTime ) //utc->local
{
    struct tm localTm, utcTm;
    time_t localTimeT, utcTimeT;

    if (utcTime == (time_t)-1) return 0;
    if (utcTime == (time_t)0) return 0;

    struct tm lTm, uTm;
    localtime_s(&lTm, &utcTime );
    gmtime_s( &uTm, &utcTime );

    memcpy( &localTm, &lTm, sizeof( struct tm ) );
    memcpy( &utcTm, &uTm, sizeof( struct tm ) );

    localTimeT = mktime( &localTm );
    if( localTm.tm_isdst )
    {
        localTimeT += 3600;
    }

    utcTimeT = mktime( &utcTm );

    if( localTimeT > utcTimeT )
    {
        return utcTime + localTimeT - utcTimeT; 
    }
    else
    {
        return utcTime - (utcTimeT - localTimeT);
    }
}