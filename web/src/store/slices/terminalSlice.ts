import { PayloadAction, createSlice } from "@reduxjs/toolkit"

interface IInitialState {
    outputTextBuffer: Array<string>
    isClearTerminal: boolean
    isInput: boolean
    inputText: string
}

const initialState: IInitialState = {
    outputTextBuffer: [],
    isClearTerminal: false,
    isInput: false,
    inputText: ''
}

const terminalSlice = createSlice({
    name: 'terminal',
    initialState,
    reducers: {
        enableTerminalInput(state) {
            state.isInput = true
        },
        disableTerminalInput(state) {
            state.isInput = false
        },
        addTerminalOutputMessage(state, action: PayloadAction<string>) {
            state.outputTextBuffer.push(action.payload)
        },
        deleteFirstTerminalOutputMessage(state) {
            if (state.outputTextBuffer.length > 0) {
                state.outputTextBuffer.shift()
            }
        },
        enableClearTerminal(state) {
            state.isClearTerminal = true
        },
        disableClearTerminal(state) {
            state.isClearTerminal = false
        },
        addTerminalInputText(state, action: PayloadAction<string>) {
            state.inputText += action.payload
        },
        deleteLastTerminalInputSymbol(state) {
            state.inputText = state.inputText.slice(0, state.inputText.length - 1)
        },
        clearTerminalInputText(state) {
            state.inputText = ''
        }
    }
})

export const {
    enableTerminalInput, disableTerminalInput,
    addTerminalOutputMessage, deleteFirstTerminalOutputMessage,
    enableClearTerminal, disableClearTerminal,
    addTerminalInputText, deleteLastTerminalInputSymbol, clearTerminalInputText
} = terminalSlice.actions
export default terminalSlice.reducer