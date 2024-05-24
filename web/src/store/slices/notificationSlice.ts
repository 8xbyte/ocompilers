import { PayloadAction, createSlice } from "@reduxjs/toolkit"

interface IInitialState {
    messages: Array<string>
}

const initialState: IInitialState = {
    messages: []
}

const notificationSlice = createSlice({
    name: 'notification',
    initialState,
    reducers: {
        addNotificationMessage(state, action: PayloadAction<string>) {
            state.messages.push(action.payload)
        },
        deleteFirstNotificationMessage(state) {
            state.messages.shift()
        }
    }
})

export const { addNotificationMessage, deleteFirstNotificationMessage } = notificationSlice.actions
export default notificationSlice.reducer