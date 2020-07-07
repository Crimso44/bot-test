import * as moment from 'moment';

export const formatDate = (dateTime: string): string => {
    if (moment(dateTime, 'YYYY-MM-DDTHH:mm').isValid()) {
        return moment(dateTime, 'YYYY-MM-DDTHH:mm').format('DD.MM.YYYY');
    }
    else {
        return null;
    }
}

export const formatDateTime = (dateTime: string, seconds: boolean = false): string => {
    var ss = seconds?':ss':'';
    if (moment(dateTime, `YYYY-MM-DDTHH:mm${ss}`).isValid()) {
        return moment(dateTime, `YYYY-MM-DDTHH:mm${ss}`).format(`DD.MM.YYYY HH:mm${ss}`);
    }
    else {
        return null;
    }
}
