import React, { ButtonHTMLAttributes, DetailedHTMLProps } from 'react'

import './style.scss'

interface IProps extends DetailedHTMLProps<ButtonHTMLAttributes<HTMLButtonElement>, HTMLButtonElement> {
    iconName?: string
    iconFile?: string
    className?: string
}

const IconButton: React.FC<IProps> = ({ iconName, iconFile, className, ...others }) => {
    if (iconName) {
        return (
            <button className={['default-icon-button', 'codicon', `codicon-${iconName}`, className].join(' ')} {...others}></button>
        )
    } else {
        return (
            <button className={['default-icon-button', className].join(' ')} {...others}>
                <img className='default-icon' alt={iconFile} src={iconFile} />
            </button>
        )
    }
}

export default IconButton