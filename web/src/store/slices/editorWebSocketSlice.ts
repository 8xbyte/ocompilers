import { PayloadAction, createSlice } from "@reduxjs/toolkit"
import { IRuntimeMessage } from "../../types/message"


interface IInitialState {
    messages: Array<IRuntimeMessage>
    isOpen: boolean
}

const initialState: IInitialState = {
    messages: [],
    isOpen: false
}

const editorWebSocketSlice = createSlice({
    name: 'editorWebSocket',
    initialState,
    reducers: {
        openEditorWebSocket(state) {
            state.isOpen = true
        },
        closeEditorWebSocket(state) {
            state.isOpen = false
        },
        addEditorWebSocketMessage(state, action: PayloadAction<IRuntimeMessage>) {
            state.messages.push(action.payload)
        },
        deleteFirstEditorWebSocketMessage(state) {
            if (state.messages.length > 0) {
                state.messages.shift()
            }
        }
    }
})

export const {
    openEditorWebSocket, closeEditorWebSocket,
    addEditorWebSocketMessage, deleteFirstEditorWebSocketMessage
} = editorWebSocketSlice.actions
export default editorWebSocketSlice.reducer