import { PayloadAction, createSlice } from "@reduxjs/toolkit"

interface IInitialState {
    text: string
    currentLanguage: string
}

const initialState: IInitialState = {
    text: '',
    currentLanguage: 'cpp'
}

const editorSlice = createSlice({
    name: 'monacoEditor',
    initialState,
    reducers: {
        setEditorText(state, action: PayloadAction<string>) {
            state.text = action.payload
        },
        setEditorCurrentLanguage(state, action: PayloadAction<string>) {
            state.currentLanguage = action.payload
        }
    }
})

export const { setEditorText, setEditorCurrentLanguage } = editorSlice.actions
export default editorSlice.reducer