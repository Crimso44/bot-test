
export interface IMessageButtonProps {
    category:string;
    text: string;
    context: string
    onButtonClick: (category: string, context: string, text: string, isAddQuestion: boolean) => void
}