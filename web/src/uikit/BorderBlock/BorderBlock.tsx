import React, { DetailedHTMLProps, HTMLAttributes } from 'react'

import './style.scss'

interface IProps extends DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement> {
    className?: string
    children?: React.ReactNode
}

const BorderBlock: React.FC<IProps> = ({ className, children, ...others }) => {
    return (
        <div className={['default-border-block', className].join(' ')} {...others}>{children}</div>
    )
}

export default BorderBlock