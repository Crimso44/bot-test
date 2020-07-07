
export class ScrollService {

    currentIteration: number = 0;
    animIterations: number = 0;
    offset: number = 0;
    start: number = 0;

    constructor() {
    }

    scrollDown = (el: Element): void => {
        el.scrollTop = el.scrollHeight - el.clientHeight;
    }

    scrollTo = (el: Element, to: number): void => {
        el.scrollTop = to;
    }

    scrollAnimate = (el: Element, from: number, to: number): void => {

        const getRandomInt = (min, max): number => {
            return Math.floor(Math.random() * (max - min)) + min;
        }

        this.animIterations = getRandomInt(15, 20);
        this.currentIteration = 0;
        this.start = from;
        this.offset = to - from;

        const easeOutCubic = (): number => {
            return this.start + this.offset * (Math.pow(this.currentIteration / this.animIterations - 1, 3) + 1);
        }

        const sc = (): void => {
            const value = easeOutCubic();
            el.scrollTop = value;
            this.currentIteration++;

            if (this.currentIteration < this.animIterations)
            {
                requestAnimationFrame(sc);
            }
        }

        requestAnimationFrame(sc);
    }
}