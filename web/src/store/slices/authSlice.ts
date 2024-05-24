import { PayloadAction, createSlice } from "@reduxjs/toolkit"
import { IMessage, login, register } from "../../api/auth"
import { Status } from "../../types/status"

interface IInitialState {
    isAuth: boolean
    login: {
        status: Status
        result: IMessage | null,
        error: string | null
    }
    register: {
        status: Status
        result: IMessage | null,
        error: string | null
    }
}

const initialState: IInitialState = {
    isAuth: localStorage.getItem('isAuth') === 'true' ? true : false,
    login: {
        status: 'none',
        result: null,
        error: null
    },
    register: {
        status: 'none',
        result: null,
        error: null
    }
}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setIsAuth(state, action: PayloadAction<boolean>) {
            localStorage.setItem("isAuth", action.payload ? "true" : "false")
            state.isAuth = action.payload
        }
    },
    extraReducers(builder) {
        builder.addCase(login.pending, (state, action) => {
            state.login.status = 'loading'
        }).addCase(login.fulfilled, (state, action) => {
            state.login.status = 'success'
            state.login.result = action.payload
            state.isAuth = true
        }).addCase(login.rejected, (state, action) => {
            state.login.status = 'failed'
            state.login.error = action.payload as string
            state.isAuth = false
        })

        builder.addCase(register.pending, (state, action) => {
            state.register.status = 'loading'
        }).addCase(register.fulfilled, (state, action) => {
            state.register.status = 'success'
            state.register.result = action.payload
            state.isAuth = true
        }).addCase(register.rejected, (state, action) => {
            state.register.status = 'failed'
            state.register.error = action.payload as string
            state.isAuth = false
        })
    },
})

export const { setIsAuth } = authSlice.actions
export default authSlice.reducer