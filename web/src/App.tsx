import React from 'react'

import EditorBlock from './components/EditorBlock/EditorBlock'
import WorkspaceBlock from './components/WorkspaceBlock/WorkspaceBlock'

import './App.scss'
import { useAppSelector } from './store/store'
import AuthPopup from './components/AuthPopup/AuthPopup'
import Notification from './components/Notification/Notification'

const App: React.FC = () => {
    const data = useAppSelector(state => state.data)
    const workspaceStruct = useAppSelector(state => state.workspaceStruct)

    return (
        <div className='app'>
            <Notification timeout={3000}/>
            <AuthPopup isShow={data.isShowAuthPopup} />
            <WorkspaceBlock />
            {workspaceStruct.openned === null ? null : < EditorBlock />}
        </div>
    )
}

export default App
