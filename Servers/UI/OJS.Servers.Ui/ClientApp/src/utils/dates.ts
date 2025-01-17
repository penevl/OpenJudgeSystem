import { differenceInDays, intervalToDuration } from 'date-fns';
import moment from 'moment';

const defaultDateTimeFormat = 'HH:MM, DD/MMM/yyyy';
const defaultPreciseDateTimeFormat = 'DD/MMM/yyyy, HH:mm:ss';

const calculateTimeUntil = (date: Date) => intervalToDuration({
    start: new Date(),
    end: date,
});

const preciseFormatDate = (
    date: Date,
    formatString = defaultPreciseDateTimeFormat,
) => moment(date).utc(true).local().format(formatString);

const formatDate = (
    date: Date,
    formatString = defaultDateTimeFormat,
) => (moment().diff(date, 'days') > 3
    ? preciseFormatDate(date, formatString)
    : moment(date).utc(true).local().fromNow());

const getCurrentTimeInUTC = () => {
    const now = moment().utc();
    return new Date(
        now.year(),
        now.month(),
        now.date(),
        now.hour(),
        now.minute(),
        now.second(),
        now.millisecond(),
    );
};
const convertToSecondsRemaining = (date: Date) => {
    const currentDate = getCurrentTimeInUTC();
    const { hours, minutes, seconds } = intervalToDuration({
        start: currentDate,
        end: date,
    });

    const daysRemaining = differenceInDays(date, currentDate);

    const hoursRemaining = daysRemaining * 24 + (hours ?? 0);

    const minutesRemaining = hoursRemaining * 60 + (minutes ?? 0);

    return minutesRemaining * 60 + (seconds ?? 0);
};

// This function converts a given duration in seconds into a time object that includes
// days, hours, minutes, and seconds.
// The function first calculates the number of whole days within the duration
// The remaining seconds are computed using the modulus operator (%)
// The function 'intervalToDuration' is then used to break down the remaining seconds into h, m, s
const secondsToFullTime = (duration: number) => {
    // Number of seconds in a day: 86400 (60 seconds * 60 minutes * 24 hours)
    const days = Math.floor(duration / 86400);
    const remainingSeconds = duration % 86400;
    const { hours: hoursInitial, minutes: minutesInitial, seconds: secondsInitial } =
        intervalToDuration({ start: 0, end: remainingSeconds * 1000 });

    const hours = hoursInitial ?? 0;

    const minutes = minutesInitial ?? 0;

    const seconds = secondsInitial ?? 0;

    return { days, hours, minutes, seconds };
};

interface IConvertToTwoDigitValuesParamType {
    days: number;
    hours: number;
    minutes: number;
    seconds: number;
}

const convertToTwoDigitValues = ({
    days: daysValue,
    hours: hoursValue,
    minutes: minutesValue,
    seconds: secondsValue,
}: IConvertToTwoDigitValuesParamType) => {
    const days = daysValue >= 10
        ? daysValue.toString()
        : `0${daysValue}`;
    const hours = hoursValue >= 10
        ? hoursValue.toString()
        : `0${hoursValue}`;

    const minutes = minutesValue >= 10
        ? minutesValue.toString()
        : `0${minutesValue}`;

    const seconds = secondsValue >= 10
        ? secondsValue.toString()
        : `0${secondsValue}`;

    return {
        days,
        hours,
        minutes,
        seconds,
    };
};

export default {
    formatDate,
    preciseFormatDate,
    secondsToFullTime,
    calculateTimeUntil,
    convertToSecondsRemaining,
    convertToTwoDigitValues,
    getCurrentTimeInUtc: getCurrentTimeInUTC,
};

export {
    formatDate,
    preciseFormatDate,
    secondsToFullTime,
    calculateTimeUntil,
    convertToSecondsRemaining,
    convertToTwoDigitValues,
    getCurrentTimeInUTC,
};
