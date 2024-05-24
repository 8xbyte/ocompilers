import { PayloadAction, createSlice } from "@reduxjs/toolkit"

interface IInitialState {
    isShowAuthPopup: boolean
    isAuthPopupLogin: boolean
}

const initialState: IInitialState = {
    isShowAuthPopup: false,
    isAuthPopupLogin: true
}

const dataSlice = createSlice({
    name: 'data',
    initialState,
    reducers: {
        setIsShowAuthPopup(state, action: PayloadAction<boolean>) {
            state.isShowAuthPopup = action.payload
        },
        setIsAuthPopupLogin(state, action: PayloadAction<boolean>) {
            state.isAuthPopupLogin = action.payload
        },
    }
})

export const { setIsShowAuthPopup, setIsAuthPopupLogin } = dataSlice.actions
export default dataSlice.reducer