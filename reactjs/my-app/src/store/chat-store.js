import * as csharpQueue from '../csharp-message-queue-factory';
const RECEIVED_MESSAGE = "RECEIVED_MESSAGE";
const defaultState = {
    messages:[],
    profile:{},
    toastMessage:"",
}

export default function ChatStore(state = defaultState, action) {
    console.log(action);
    switch (action.type) {
        case csharpQueue.NOTIFY_USER_CONNECTED:
            return {
                    ...state,
                    profile: action.params.profile,
                    toastMessage: `${action.params.profile.name} connected`
                 };
        case RECEIVED_MESSAGE:
                var messages = state.messages;
                messages.push(action.params);
            return {
                    ...state,
                    messages: messages,
                    toastMessage: ''
                 };
        default: return state;
    }
}