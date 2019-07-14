import {Switch, Route, Redirect} from 'react-router-dom';

export const routes = (
    <Switch>
        <Redirect exact from={'/'} to={`/`}/>
        <Route exact path="/" component={WorkTitle}/>
    </Switch>
);