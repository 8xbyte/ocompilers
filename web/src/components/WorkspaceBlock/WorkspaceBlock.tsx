import React from 'react'

import ExplorerBlock from '../ExplorerBlock/ExplorerBlock'
import UserBlock from '../UserBlock/UserBlock'

import './style.scss'

const WorkspaceBlock: React.FC = () => {
    return (
        <div className='current-workspace-block'>
            <UserBlock />
            <ExplorerBlock />
        </div>
    )
}

export default WorkspaceBlock