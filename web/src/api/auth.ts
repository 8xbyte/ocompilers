import { createAsyncThunk } from "@reduxjs/toolkit"
import axios, { AxiosError } from 'axios'
import { setIsAuth } from '../store/slices/authSlice'
import { IError } from "../types/error"

export interface IMessage {
    status: string
}

interface ILoginData {
    email: string
    password: string
}

export const login = createAsyncThunk('auth/login', async (data: ILoginData, thunkApi) => {
    try {
        const response = await axios.post<IMessage>('https://astra.oregona.ru/api/auth/login', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})

interface IRegisterData {
    email: string
    password: string
}

export const register = createAsyncThunk('auth/register', async (data: IRegisterData, thunkApi) => {
    try {
        const response = await axios.post<IMessage>('https://astra.oregona.ru/api/auth/register', data, {
            withCredentials: true
        })
        return thunkApi.fulfillWithValue(response.data)
    } catch (error) {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError<IError>
            if (axiosError.response?.status === 403) thunkApi.dispatch(setIsAuth(false))
            return thunkApi.rejectWithValue(axiosError.response?.data.message)
        } else {
            return thunkApi.rejectWithValue(error)
        }
    }
})