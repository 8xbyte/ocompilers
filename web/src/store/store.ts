import { configureStore } from '@reduxjs/toolkit'
import { TypedUseSelectorHook, useDispatch, useSelector, useStore } from 'react-redux'

import authSlice from './slices/authSlice'
import dataSlice from './slices/dataSlice'
import editorWebSocketSlice from './slices/editorWebSocketSlice'
import monacoEditorSlice from './slices/monacoEditorSlice'
import notificationSlice from './slices/notificationSlice'
import terminalSlice from './slices/terminalSlice'
import workspaceSlice from './slices/workspaceSlice'
import workspaceStructSlice from './slices/workspaceStructSlice'

const store = configureStore({
    reducer: {
        auth: authSlice,
        editorWebSocket: editorWebSocketSlice,
        monacoEditor: monacoEditorSlice,
        terminal: terminalSlice,
        data: dataSlice,
        notification: notificationSlice,
        workspace: workspaceSlice,
        workspaceStruct: workspaceStructSlice
    }
})

export type AppStore = typeof store
export type RootState = ReturnType<AppStore['getState']>
export type AppDispatch = AppStore['dispatch']

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector
export const useAppStore: () => AppStore = useStore

export default store