import * as actionTypes from '../action_type';

const defaultState = {
    messages:[],
    profile:{},
    toastMessage:"",
}

export default function ChatStore(state = defaultState, action) {
    switch (action.type) {
        case actionTypes.CSHARP_NOTIFY_USER_CONNECTED:
            return {
                    ...state,
                    profile: action.params.profile,
                    toastMessage: `${action.params.profile.name} connected`
                 };
        case actionTypes.CHAT_MESSAGE:
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