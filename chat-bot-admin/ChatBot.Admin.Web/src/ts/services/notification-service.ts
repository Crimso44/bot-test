import * as Rx from 'rxjs';
import { QueueingSubject } from 'queueing-subject';
//import websocketConnect from '../classes/rxWebSocket';
import * as _ from 'lodash';

declare const Notification;

export interface INotification {
    id: string;
    text: string;
    isRead: boolean;
    type?: string;
    dalay?: number;
}

export interface INotificationService {

    send(notificationText: string, type: string, delay: number): void
    put(notificationText: string): void;
    remove(notification: INotification): void;
    markAsRead(notification: INotification): void;
    markAsUnRead(notification: INotification): void;
    getNotificationList(skip: number, take: number): INotification[];
    watch(subscriber: (INotification) => void, filter?: (INotification) => boolean): void;
    getStream(): Rx.Subject<INotification>
}
class $NotificationService implements INotificationService {

    static notificationList: INotification[] = [];
    static stream: Rx.Subject<INotification> = new Rx.Subject<INotification>();
    OSNotification: boolean = true;

    constructor() {
    }

    OSnotificationShow(notification: INotification) {
        if (("Notification" in window)) {

            if (Notification.permission === "granted") {
                var OSNotification = new Notification(notification.text);
            }

            else if (Notification.permission !== 'denied') {
                Notification.requestPermission(function (permission) {
                    if (permission === "granted") {
                        var notification = new Notification(notification.text);
                    }
                });
            }
        }
    }

    send(notificationText: string, type: string = 'normal', delay: number = Infinity, closeable: boolean = false): void {
        var _notification = { id: _.uniqueId().toString(), text: notificationText, isRead: false, type: type, delay: delay, closeable: closeable };
        $NotificationService.stream.next(_notification);
    }

    put(notificationText: string): void {
        var _notification = { id: _.uniqueId().toString(), text: notificationText, isRead: false };
        $NotificationService.notificationList.push(_notification);
        $NotificationService.stream.next(_notification);
        if (this.OSNotification) { this.OSnotificationShow(_notification) }
    }
    remove(notification: INotification): void {
        _.remove($NotificationService.notificationList, n => n.id === notification.id);
        $NotificationService.stream.next(notification);
    }
    markAsRead(notification: INotification): void {
        _.forEach($NotificationService.notificationList, n => { n.id === notification.id ? n.isRead = true : null });
        $NotificationService.stream.next(notification);
    }
    markAsUnRead(notification: INotification): void {
        _.forEach($NotificationService.notificationList, n => { n.id === notification.id ? n.isRead = false : null });
        $NotificationService.stream.next(notification);
    }
    getNotificationList(skip: number, take: number): INotification[] {
        return _.slice($NotificationService.notificationList, skip, skip + take);
    }

    watch(subscriber: (INotification) => void, filter: (Notification) => boolean = () => true): void {
        $NotificationService.stream.filter(filter).subscribe(subscriber);
    };

    getStream(): Rx.Subject<INotification> {
        return $NotificationService.stream;
    }
}

const NotificationService: INotificationService = new $NotificationService();
export default NotificationService;